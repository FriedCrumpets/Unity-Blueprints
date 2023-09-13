using UnityEngine.InputSystem;

namespace Blueprints.Entities.Systems.Inventory.Interfaces
{
    public interface IQuickslotController
    {
        void QuickSlot(InputActionPhase phase);
    }
}