using System;
using System.Collections;
using Blueprints.Components;
using Blueprints.XR;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public enum XRDevice : byte
{
    None,
    Head,
    LeftHand,
    RightHand
}

public class XRHead { }
public class XRLeftHand { }
public class XRRightHand { }

public class XRTrackedDevice : MonoComponent,
    XRInput.IXRIHeadTrackingActions, XRInput.IXRILeftHandTrackingActions, XRInput.IXRIRightHandTrackingActions
{
    /// <summary>
    /// These bit flags correspond with <c>UnityEngine.XR.InputTrackingState</c>
    /// but that enum is not used to avoid adding a dependency to the XR module.
    /// Only the Position and Rotation flags are used by this class, so velocity and acceleration flags are not duplicated here.
    /// </summary>
    [Flags]
    enum TrackingStates
    {
        /// <summary>
        /// Position and rotation are not valid.
        /// </summary>
        None,

        /// <summary>
        /// Position is valid.
        /// See <c>InputTrackingState.Position</c>.
        /// </summary>
        Position = 1 << 0,

        /// <summary>
        /// Rotation is valid.
        /// See <c>InputTrackingState.Rotation</c>.
        /// </summary>
        Rotation = 1 << 1,
    }
    
    private Transform _transform;
    private Vector3 _newPosition;
    private Quaternion _newRotation;
    private bool _active;

    public void Enable()
    {
        Active = true;
        SetCallbacks(Device);
    }

    public void Init(IComponent master)
    {
        base.Init(master, false);
        Migrate(master, RetrieveType(Device));
    }

    public void Disable()
    {
        Active = false;
        SetCallbacks(XRDevice.None);
    }

    private IEnumerator UpdatePose()
    {
        while (Active)
        {
            var positionValid = (CurrentTrackingState & TrackingStates.Position) != 0;
            var rotationValid = (CurrentTrackingState & TrackingStates.Rotation) != 0;

            if (positionValid && TrackingType is TrackedPoseDriver.TrackingType.PositionOnly
                    or TrackedPoseDriver.TrackingType.RotationAndPosition)
                _transform.localPosition = _newPosition;
            
            if(rotationValid && TrackingType is TrackedPoseDriver.TrackingType.RotationOnly 
                   or TrackedPoseDriver.TrackingType.RotationAndPosition)
                _transform.localRotation = _newRotation;
            
            yield return null;
        }
    }

    [field: SerializeField] public XRDevice Device { get; private set; }
    [field: SerializeField] public TrackedPoseDriver.TrackingType TrackingType { get; set; }

    public bool Active
    {
        get => _active;
        private set
        {
            if(value)
                StartCoroutine(UpdatePose());
            
            _active = value;
        }
    }

    private TrackingStates CurrentTrackingState { get; set; } = TrackingStates.Position | TrackingStates.Rotation;

    public void OnPosition(InputAction.CallbackContext context)
        => _newPosition = context.phase == InputActionPhase.Canceled ?
            Vector3.zero : context.ReadValue<Vector3>();

    public void OnRotation(InputAction.CallbackContext context)
        => _newRotation = context.phase == InputActionPhase.Canceled ?
            Quaternion.identity : context.ReadValue<Quaternion>();

    public void OnTrackingState(InputAction.CallbackContext context)
        => CurrentTrackingState = context.phase == InputActionPhase.Canceled ?
            TrackingStates.None : (TrackingStates)context.ReadValue<int>();

    private static Type RetrieveType(XRDevice device)
    {
        return device switch
        {
            XRDevice.None => null,
            XRDevice.Head => typeof(XRHead),
            XRDevice.LeftHand => typeof(XRLeftHand),
            XRDevice.RightHand => typeof(XRRightHand),
            _ => throw new ArgumentOutOfRangeException(nameof(device), device, null)
        };
    }
    
    private void SetCallbacks(XRDevice device)
    {
        Action<XRInput> action = device switch
        {
            XRDevice.None => (input) =>
            {
                input.XRIHeadTracking.RemoveCallbacks(this);
                input.XRILeftHandTracking.RemoveCallbacks(this);
                input.XRIRightHandTracking.RemoveCallbacks(this);
            },
            XRDevice.Head => (input) =>
            {
                input.XRIHeadTracking.AddCallbacks(this);
                input.XRILeftHandTracking.RemoveCallbacks(this);
                input.XRIRightHandTracking.RemoveCallbacks(this);
            },
            XRDevice.LeftHand => (input) =>
            {
                input.XRIHeadTracking.RemoveCallbacks(this);
                input.XRILeftHandTracking.AddCallbacks(this);
                input.XRIRightHandTracking.RemoveCallbacks(this);
            },
            XRDevice.RightHand => (input) =>
            {
                input.XRIHeadTracking.RemoveCallbacks(this);
                input.XRILeftHandTracking.RemoveCallbacks(this);
                input.XRIRightHandTracking.AddCallbacks(this);
            },
            _ => throw new ArgumentOutOfRangeException(nameof(device), device, null)
        };

        IComponent.SendMessage(Master, action);
    }
}
