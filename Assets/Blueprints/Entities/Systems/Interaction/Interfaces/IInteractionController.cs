using UnityEngine.InputSystem;

namespace Blueprints.Entities.Systems.Interaction.Interfaces
{
    public interface IInteractionController
    {
        void OnInteract(Actor actor, InputActionPhase phase);
    }
}