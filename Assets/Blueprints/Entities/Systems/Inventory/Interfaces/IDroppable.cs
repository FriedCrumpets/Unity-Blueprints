using UnityEngine.InputSystem;

namespace Blueprints.Entities.Systems.Inventory.Interfaces
{
    public interface IDroppableController
    {
        void OnDrop(InputActionPhase phase);
    }
}