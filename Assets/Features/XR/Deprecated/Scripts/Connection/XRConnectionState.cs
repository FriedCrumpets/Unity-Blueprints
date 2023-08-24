using Blueprints.EventBus;
using UnityEngine.InputSystem;

namespace Features.XR
{
    public enum XRConnection
    {
        XRConnected,
        XRAdded,
        XRRemoved,
        XRDisconnected,
    }
    
    public class XRConnectionState : TBus
    {
        public XRConnection ConnectionState { get; private set; }

        private const string _XR_HEAD_DEVICE_DESCRIPTION = "OpenXR Head Tracking"; 
        
        private void OnEnable() 
            => InputSystem.onDeviceChange += SearchForXRDevice;

        private void OnDisable() 
            => InputSystem.onDeviceChange -= SearchForXRDevice;

        private void SearchForXRDevice(InputDevice device, InputDeviceChange change)
        {
            var newConnectionState = change switch
            {
                InputDeviceChange.Reconnected when CheckDeviceForXRHeadTracking(device) => XRConnection.XRConnected,
                InputDeviceChange.Added when CheckDeviceForXRHeadTracking(device) => XRConnection.XRAdded,
                InputDeviceChange.Removed when CheckDeviceForXRHeadTracking(device) => XRConnection.XRRemoved,
                InputDeviceChange.Disconnected when CheckDeviceForXRHeadTracking(device) => XRConnection.XRDisconnected,
                _ => ConnectionState
            };
            
            if (ConnectionState == newConnectionState) { return; }

            ConnectionState = newConnectionState;
            Publish<XRConnection>(ConnectionState);
        }

        private static bool CheckDeviceForXRHeadTracking(InputDevice device)
        {
            return device.description.ToString().Contains(_XR_HEAD_DEVICE_DESCRIPTION);
        }
    }   
}