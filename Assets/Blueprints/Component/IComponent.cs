using System;
using System.Collections.Generic;
using System.Linq;
using Blueprints.ServiceLocator;
using UnityEngine;

namespace Blueprints.Components
{
    public interface IComponent : IService
    {
        IComponent Master { get; set; }
        Action<IComponent> OnComponentCreated { get; }
        Locator Locator { get; }
        List<KeyValuePair<Type, Action<IComponent>>> StoredCommands { get; }
     
        static void Receive<T>(T component, Action<T> message) where T : IComponent
            => message?.Invoke(component); 
        
        T Get<T>() where T : IService; 
        T Add<T>(T service) where T : IService; 
        
        static bool SendMessage<T>(IComponent master, Action<T> action) where T : IComponent
        {
            var component = master.Get<T>();
            
            if (component != null)
                Receive(component, action);
            else
                master.StoredCommands.Add(
                    new KeyValuePair<Type, Action<IComponent>>(
                        typeof(T), component => action?.Invoke((T)component)));
            
            return component != null;
        }
        
        bool Remove<T>(T service) where T : IService; 
    }

    public class TestComponentIdea : IComponent
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
        public Locator Locator { get; }
        public List<KeyValuePair<Type, Action<IComponent>>> StoredCommands { get; }

        public TestComponentIdea(IComponent master = null)
        {
            Master = master ?? this;
            StoredCommands = new();
        }
        
        public T Get<T>() where T : IService
            => Locator.Get<T>();

        public T Add<T>(T service) where T : IService
            => Locator.Provide(service);

        public bool Remove<T>(T service) where T : IService
            => Locator.Remove<T>();
    }
    
    public class MonoTestComponent : MonoBehaviour, IComponent
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