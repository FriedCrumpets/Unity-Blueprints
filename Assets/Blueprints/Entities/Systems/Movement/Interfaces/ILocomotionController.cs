using UnityEngine.InputSystem;

namespace Blueprints.Entities
{
    public interface ILocomotionController
    {
        void OnCrouch(InputActionPhase phase);
        void OnSprint(InputActionPhase phase);
        void OnJump(InputActionPhase phase);
    }
}