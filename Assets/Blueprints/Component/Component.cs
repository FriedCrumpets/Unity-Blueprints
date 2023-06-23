using System;
using System.Collections.Generic;
using Blueprints.ServiceLocator;
using Blueprints.Utility;

namespace Blueprints.Components
{
    public abstract class Component : IComponent
    {
        private IComponent _master;

        public Component(IComponent master = null)
        {
            Master = master ?? this;
            Components = master != null ? master.Components : new();
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
        
        public Action<IComponent> ComponentCreated { get; }
        public Locator Components { get; }
        public List<KeyValuePair<Type, Action<IComponent>>> StoredCommands { get; }
        
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