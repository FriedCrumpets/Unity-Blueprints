using System;
using System.Collections.Generic;
using Blueprints.ServiceLocator;
using UnityEngine;

namespace Blueprints.Components
{
    public abstract class MonoComponent : MonoBehaviour, IService, IDisposable
    {
        public Action<Type> OnComponentCreated;
        
        private Locator _locator;

        public List<KeyValuePair<Type, Action<Component>>> StoredCommands { get; private set; }
        
        private void Awake()
        {
            _locator = new();
            StoredCommands = new();
            ConstructComponents();
            Init();
        }

        protected abstract void Init();
        
        public T GetService<T>() where T : IService
            => _locator.Get<T>();
                
        public T AddService<T>(T component) where T : IService
            => _locator.Provide(component);
        
        public bool RemoveService<T>(T component) where T : IService
            => _locator.Remove<T>();

        protected abstract void ConstructComponents();

        private void OnDestroy()
        {
            _locator.Dispose();
            Dispose();
        }

        public abstract void Dispose();
    }
}