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

        private void Awake()
        {
            Master = this;
            Components = new();
            StoredCommands = new();
            ComponentCreated = component => { };
        }
        
        public IComponent Master
        {
            get => _master;
            set
            {
                _master?.Remove(this);
                value.Add(this);
                value.ComponentCreated?.Invoke(this);

                foreach (var pair in value.StoredCommands)
                {
                    if (pair.Key == GetType())
                    {
                        IComponent.Receive(value, pair.Value);
                        value.StoredCommands.Remove(pair);
                    }
                }

                _master = value;
            }
        }
        
        public Action<IComponent> ComponentCreated { get; private set; }
        public Locator Components { get; private set; }
        public List<KeyValuePair<Type, Action<IComponent>>> StoredCommands { get; private set; }
        
        public T Get<T>() where T : IService
            => Components.Get<T>();
        
        public bool TryGet<T>(out T value) where T : IService
            => Components.TryGet(out value);

        public T Add<T>(T service) where T : IService
            => Components.Provide(service);

        public bool Remove<T>(T service) where T : IService
            => Components.Remove<T>();
    }
}