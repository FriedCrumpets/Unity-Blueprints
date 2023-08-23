using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Camera.Deprecated
{
    public interface ICameraRig 
    {
        Transform GhostCamera { get; }
        void Init(Transform target, Transform lookAt);
        void Activate(DynamicCamera camera);
        void Deactivate();
        void DeInit();
        void OnScroll(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
    }
}