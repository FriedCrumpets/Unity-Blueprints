using System;
using System.Collections.Generic;
using System.Linq;
using Blueprints.ServiceLocator;
using UnityEngine;

namespace Blueprints.Components
{
    public abstract class MonoComponent<TComponent> : MonoBehaviour, IService where TComponent : UnityEngine.Component
    {
        public static event Action<Type> OnMonoComponentCreated;

        public event Action<TComponent> OnInitialised;
        
        protected static TComponent Component { get; private set; }
        
        private static Locator _locator;
        private static  List<KeyValuePair<Type, Action<MonoComponent<TComponent>>>> _storedCommands;

        protected virtual void Awake()
        {
            Locator.Provide(this);    
            OnMonoComponentCreated?.Invoke(GetType());
            
            foreach (var pair in StoredCommands.Where(pair => pair.Key == GetType()))
            {
                Receive(this, pair.Value);
                StoredCommands.Remove(pair);
            }
        }

        public virtual void Initialise(TComponent component)
        {
            Component = component;
            OnInitialised?.Invoke(component);
        }

        private static Locator Locator 
            => _locator ??= new();
        
        private static List<KeyValuePair<Type, Action<MonoComponent<TComponent>>>> StoredCommands 
            => _storedCommands ??= new();

        protected static T Get<T>() where T : MonoComponent<TComponent>
            => Locator.Get<T>();

        protected static bool Send<T>(Action<T> action) where T : MonoComponent<TComponent>
        {
            var component = Get<T>();
            
            if (component != null)
                Receive(component, action);
            else
                StoredCommands.Add(
                    new KeyValuePair<Type, Action<MonoComponent<TComponent>>>(
                        typeof(T), component => action?.Invoke((T)component))
                );
            
            return component != null;
        }

        private static void Provide<T>(T component) where T : MonoComponent<TComponent>
            => Locator.Provide(component);
        
        private static void Receive<T>(T component, Action<T> action)
            => action?.Invoke(component);
    }
}