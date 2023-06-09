using System;
using System.Collections.Generic;
using System.Linq;
using Blueprints.ServiceLocator;
using Blueprints.Utility;
using UnityEngine;

namespace Blueprints.Components
{
    public abstract class MonoComponent : MonoBehaviour, IComponent
    {
        private IComponent _master;

        public IComponent Master
        {
            get => _master;
            set
            {
                _master?.Remove(this);
                value.Add(this);
                value.OnComponentCreated?.Invoke(this);
                
                foreach (var pair in value.StoredCommands.Where(pair => pair.Key == GetType()))
                {
                    IComponent.Receive(value, pair.Value);
                    value.StoredCommands.Remove(pair);
                }
                
                _master = value;
            }
        }
        
        public Action<IComponent> OnComponentCreated { get; }
        public Locator Locator { get; private set; }
        public List<KeyValuePair<Type, Action<IComponent>>> StoredCommands { get; private set; }

        private void Awake()
        {
            Master = this;
            Locator = new();
            StoredCommands = new();
        }
        
        public T Get<T>() where T : IService
            => Locator.Get<T>();

        public T Add<T>(T service) where T : IService
            => Locator.Provide(service);

        public bool Remove<T>(T service) where T : IService
            => Locator.Remove<T>();
    }
}