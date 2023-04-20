using Blueprints.ServiceLocator;
using UnityEngine;

namespace Blueprints.Components
{
    public abstract class MonoComponent : MonoBehaviour, IService
    {
        private Locator _locator;
        private Locator Locator => _locator ??= new();

        protected virtual void Awake()
        {
            ConstructComponents();
        }
        
                
        public void AddService<T>(T component) where T : IService
            => Locator.Provide(component);

        protected abstract void ConstructComponents();
    }
}