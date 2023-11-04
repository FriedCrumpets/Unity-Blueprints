using UnityEngine.InputSystem;

namespace Blueprints.Entities.Systems.Inventory.Interfaces
{
    public interface IEquippableController
    {
        bool Equipped { get; set; }
        void OnEquip(InputActionPhase phase);
        void OnUnEquip(InputActionPhase phase);
    }
}