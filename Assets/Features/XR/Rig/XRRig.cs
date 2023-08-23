using System;
using System.Collections;
using System.Collections.Generic;
using Blueprints.Components;
using Blueprints.DoD.v1;
using UnityEngine;
using UnityEngine.XR;

namespace Features.XR
{
    public class XRRig : MonoComponent
    {
        private Transform _transform;
        
        private static readonly List<XRInputSubsystem> Subsystems = new List<XRInputSubsystem>();

        [field: SerializeField] private Rig Rig { get; set; }
        [field: SerializeField] private Origin Origin { get; set; }

        public Vector3 RigInCameraSpace
            => Origin.Camera.Get().transform.InverseTransformPoint(Rig.Base.Get().transform.position);

        public Vector3 CameraInRigSpace 
            => Rig.Base.Get().InverseTransformPoint(Origin.Camera.Get().transform.position);

        public float CameraInBaseSpaceHeight
            => CameraInRigSpace.y;

        public void Init(IComponent master, Rig rig = null, Origin origin = null)
        {
            base.Init(master, false);
            
            if (rig != null)
                Rig = rig;

            if (origin != null)
                Origin = origin;

            // This will be the parent GameObject for any trackables (such as planes) for which
            // we want a corresponding GameObject.
            Origin.TrackablesParent.Set(new GameObject("Trackables").transform);
            var trackablesParent = Origin.TrackablesParent.Get();
            trackablesParent.SetParent(Rig.Base.Get(), false);
            trackablesParent.localPosition = Vector3.zero;
            trackablesParent.localRotation = Quaternion.identity;
            trackablesParent.localScale = Vector3.one;
            
            TryInitializeCamera();
            
            Domain.Add<XRRig>(typeof(Rig), Rig.Data);
            Domain.Add<XRRig>(typeof(Origin), Origin.Data);

            Origin.RequestedTrackingMode.Notifier += MoveOffsetHeight;
            Origin.Offset.Notifier += MoveOffsetHeight;
            
            Migrate(master);
        }

        public void DeInit()
        {
            Origin.RequestedTrackingMode.Notifier -= MoveOffsetHeight;
            Origin.Offset.Notifier -= MoveOffsetHeight;
            
            Domain.Remove<XRRig>(typeof(Rig));
            Domain.Remove<XRRig>(typeof(Origin));
        }
        
        protected void OnDestroy()
        {
#if XR_MODULE_AVAILABLE
            foreach (var subsystem in Subsystems)
            {
                if (subsystem != null)
                    subsystem.trackingOriginUpdated -= OnInputSubsystemTrackingOriginUpdated;
            }
#endif
            DeInit();
        }

        private void MoveOffsetHeight(TrackingMode trackingMode = TrackingMode.NotSpecified)
        {
            if (!Application.isPlaying)
                return;

            var offset = trackingMode switch
            {
                TrackingMode.NotSpecified => Origin.Offset.Get(),
                TrackingMode.Device => Origin.Offset.Get(),
                TrackingMode.Floor => 0,
                _ => throw new ArgumentOutOfRangeException(nameof(trackingMode), trackingMode, null)
            };
            
            MoveOffsetHeight(offset);
        }
        
        private void MoveOffsetHeight(float yOffset)
        {
            if (!Application.isPlaying)
                return;

            var position = Rig.Offset.Get().localPosition;
            position.y = yOffset;
            Rig.Offset.Get().localPosition = position;
        }

        private void TryInitializeCamera()
        {
            if (!Application.isPlaying)
                return;
            
            Origin.CameraState.Set(SetupCamera());
            if (Origin.CameraState.Get() == CameraState.Failed)
                StartCoroutine(RepeatCameraSetup());
        }

        private CameraState SetupCamera()
        {
            var state = CameraState.Initializing;
            
#if XR_MODULE_AVAILABLE
            SubsystemManager.GetInstances(Subsystems);

            foreach (var subsystem in Subsystems)
            {
                if (SetupCamera(subsystem) == CameraState.Initialized)
                {
                    // It is possible this could happen more than
                    // once so unregister the callback first just in case.
                    subsystem.trackingOriginUpdated -= OnInputSubsystemTrackingOriginUpdated;
                    subsystem.trackingOriginUpdated += OnInputSubsystemTrackingOriginUpdated;
                }
                else
                {
                    return CameraState.Failed;
                }
            }
#endif
            
            return CameraState.Initialized;
        }
        
#if XR_MODULE_AVAILABLE
        private CameraState SetupCamera(XRInputSubsystem subsystem)
        {
            if (subsystem == null)
                return CameraState.Failed;

            var state = CameraState.Failed;

            Action action = Origin.RequestedTrackingMode.Get() switch
            {
                TrackingMode.NotSpecified => () => Origin.CurrentTrackingMode.Set(subsystem.GetTrackingOriginMode()),
                TrackingMode.Device => TestTrackingModeValidity,
                TrackingMode.Floor => TestTrackingModeValidity,
                _ => throw new ArgumentOutOfRangeException()
            };

            action();

            void TestTrackingModeValidity()
            {
                var supported = subsystem.GetSupportedTrackingOriginModes();
                if (supported == TrackingOriginModeFlags.Unknown)
                {
                    state = CameraState.Failed;
                    return;
                }

                var convert = Origin.RequestedTrackingMode.Get() == TrackingMode.Device
                    ? TrackingOriginModeFlags.Device
                    : TrackingOriginModeFlags.Floor;

                if ((supported & convert) == 0)
                {
                    Origin.RequestedTrackingMode.Set(TrackingMode.NotSpecified);
                    Origin.CurrentTrackingMode.Set(subsystem.GetTrackingOriginMode());
                }
                else
                    state = subsystem.TrySetTrackingOriginMode(convert) ? CameraState.Initialized : CameraState.Failed;
            }
            
            if(state == CameraState.Initialized)
                MoveOffsetHeight();

            if (Origin.CurrentTrackingMode.Get() == TrackingOriginModeFlags.Device ||
                Origin.RequestedTrackingMode.Get() == TrackingMode.Device)
                state = subsystem.TryRecenter() ? CameraState.Initialized : CameraState.Failed;
            
            return state;
        }
        
        void OnInputSubsystemTrackingOriginUpdated(XRInputSubsystem inputSubsystem)
        {
            Origin.CurrentTrackingMode.Set(inputSubsystem.GetTrackingOriginMode());
            MoveOffsetHeight();
        }
#endif
        
        private IEnumerator RepeatCameraSetup()
        {
            Origin.CameraState.Set(CameraState.Initializing);
            while (Origin.CameraState.Get() != CameraState.Initialized)
            {
                yield return null;
                Origin.CameraState.Set(SetupCamera());
            }
        }
        
        public void RotateAroundCamera(float angleDegrees)
            => RotateAroundCamera(Rig.Base.Get().up, angleDegrees);
        
        public void RotateAroundCamera(Vector3 vector, float angleDegrees)
            => Rig.Base.Get().RotateAround(Origin.Camera.Get().transform.position, vector, angleDegrees);
        
        /// <summary>
        /// Rotates the Rig to a new Vector3.up position
        /// </summary>
        /// <param name="up"></param>
        public void RotateRig(Vector3 up)
        {
            if (Rig.Base.Get().up == up)
                return;
            
            Rig.Base.Get().rotation 
                = Quaternion.FromToRotation(Rig.Base.Get().up, up) * Rig.Base.Get().rotation;
        }

        public void RigCameraLookAt(Vector3 up, Vector3 forward)
        {
            RotateRig(up);
            // Project current camera's forward vector on the destination plane, whose normal vector is destinationUp.
            var projectedForward = Vector3.ProjectOnPlane(Origin.Camera.Get().transform.forward, up).normalized;

            // The angle that we want the XROrigin to rotate is the signed angle between projectedCamForward and destinationForward, after the up vectors are matched.
            var angle = Vector3.SignedAngle(projectedForward, forward, up);

            RotateAroundCamera(up, angle);
        }
        
        public void RigLookAt(Vector3 up, Vector3 forward)
        {
            RotateRig(up);
            // The angle that we want the XROrigin to rotate is the signed angle between projectedCamForward and destinationForward, after the up vectors are matched.
            var angle = Vector3.SignedAngle(Rig.Base.Get().forward, forward, up);

            RotateAroundCamera(up, angle);
        }

        /// <summary>
        /// Moves the Rig to the Provided Vector world position. Rig will be moved so that the camera is placed at the
        /// provided position.
        /// </summary>
        public void MoveToWorldLocation(Vector3 position)
        {
            var rot = Matrix4x4.Rotate(Origin.Camera.Get().transform.rotation);
            var delta = rot.MultiplyPoint3x4(RigInCameraSpace);
            Rig.Base.Get().position = delta + position;
        }
    }
}