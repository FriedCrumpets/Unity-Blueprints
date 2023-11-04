using UnityEngine.InputSystem;

namespace Blueprints.Entities.Systems.Items.Interfaces
{
    public interface IEquippedController
    {
        void OnPrimary(InputActionPhase phase);
        void OnSecondary(InputActionPhase phase);
        void OnTertiary(InputActionPhase phase);
        void OnQuaternary(InputActionPhase phase);
    }
}