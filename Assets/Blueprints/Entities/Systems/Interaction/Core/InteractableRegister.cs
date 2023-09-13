using System.Collections.Generic;
using Blueprints.Entities.Systems.Interaction.Interfaces;

namespace Blueprints.Entities.Systems.Interaction.Core
{
    public class InteractableRegister
    {
        private IDictionary<Identifier, IInteractable> _register; 
        
        public void RegisterInteractable(IInteractable interactable)
        {
            
        }

        public void DeRegisterInteractable(IInteractable interactable)
        {
            
        }
    }
}