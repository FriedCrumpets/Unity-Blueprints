using System;
using System.Collections.Generic;
using Blueprints.Utility;
using UnityEngine;

namespace Blueprints.Components
{
    public abstract class MonoComponent : MonoBehaviour, IComponent
    {
        private IComponent _master;

        protected virtual void Awake()
        {
            Master = this;
            Components = new();
            StoredCommands = new();
        }

        public void Init(IComponent master = null, bool automaticMigration = true)
        {
            AutomaticMigration = automaticMigration;
            Master = master ?? this;
        }
        
        public IComponent Master
        {
            get => _master;
            set
            {
                if (_master == null)
                {
                    _master = value;
                    return;
                } 
                
                _master?.Remove(this);
                if(AutomaticMigration)
                    Migrate(value);
            }
        }

        [field: SerializeField] public bool AutomaticMigration { get; private set; } = true;
        public Action<IComponent> ComponentCreated { get; private set; }
        public Locator Components { get; private set; }
        public List<KeyValuePair<Type, Action<IService>>> StoredCommands { get; private set; }
        
        public T Get<T>() where T : IService
            => Components.Get<T>();

        public TValue Get<TKey, TValue>() where TKey : IService
            => Components.Get<TKey, TValue>();
        
        public bool TryGet<T>(out T value) where T : IService
            => Components.TryGet(out value);

        public T Add<T>(T service) where T : IService
            => Components.Add(service);
        
        public void AddComponent<T>(T component) where T : IComponent
            => ComponentCreated?.Invoke(Components.Add(component));
        
        public void AddComponent<T>(Type type, T component) where T : IComponent
            => ComponentCreated?.Invoke(Components.Add(type, component));
        
        public bool Remove<T>(T service) where T : IService
            => Components.Remove<T>();

        protected void Migrate(IComponent newMaster, Type type = null)
        {
            if (type == null)
                newMaster.AddComponent(this);
            else
                newMaster.AddComponent(type, this);

            foreach (var pair in newMaster.StoredCommands)
            {
                if (pair.Key == GetType())
                {
                    IComponent.Receive(newMaster, pair.Value);
                    newMaster.StoredCommands.Remove(pair);
                }
            }

            _master = newMaster;
            ComponentCreated = newMaster.ComponentCreated;

            foreach (var pair in StoredCommands)
                newMaster.StoredCommands.Add(pair);

            StoredCommands = newMaster.StoredCommands;

            foreach (var component in Components.AllPairs())
            {
                if(component.Value is IComponent ic)
                    newMaster.AddComponent(component.Key, ic);
                else
                    newMaster.Components.Add(component.Key, component.Value);
            }

            Components = newMaster.Components;
        }
    }
}