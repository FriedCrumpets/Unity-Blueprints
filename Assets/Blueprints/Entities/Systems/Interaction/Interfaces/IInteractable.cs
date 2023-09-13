using UnityEngine.InputSystem;

namespace Blueprints.Entities.Systems.Interaction.Interfaces
{
    public interface IInteractable
    {
        int DefaultAnimationHash { get; }
        void OnInteract(Actor actor, InputActionPhase phase);
    }
}