using System;
using System.Collections.Generic;
using System.Linq;
using Blueprints.ServiceLocator;
using UnityEngine;

namespace Blueprints.Components
{
    public abstract class StaticComponent<TMonolith> : MonoBehaviour, IService 
        where TMonolith : UnityEngine.Component
    {
        protected static event Action<StaticComponent<TMonolith>> OnComponentCreated;
        protected static event Action<TMonolith> OnInitialised;
        
        private static Locator _locator;
        private static  List<KeyValuePair<Type, Action<StaticComponent<TMonolith>>>> _storedCommands;

        protected virtual void Awake()
        {
            var component = GetComponent<TMonolith>();
            if (component != null)
            {
                Initialise(component);
            }
            
            Locator.Provide(this);    
            OnComponentCreated?.Invoke(this);
            
            foreach (var pair in StoredCommands.Where(pair => pair.Key == GetType()))
            {
                Receive(this, pair.Value);
                StoredCommands.Remove(pair);
            }
        }
        
        private static Locator Locator 
            => _locator ??= new();
        
        private static List<KeyValuePair<Type, Action<StaticComponent<TMonolith>>>> StoredCommands 
            => _storedCommands ??= new();
        
        protected static TMonolith Monolith { get; private set; }

        protected void Initialise(TMonolith component)
        {
            Monolith = component;
            OnInitialised?.Invoke(component);
        }

        protected static T Get<T>() where T : IService
            => Locator.Get<T>();
        
        protected static void Provide<T>(T component) where T : StaticComponent<TMonolith>
            => Locator.Provide(component);
        
        protected static void Provide<T, TT>(TT service)
            where T : IService
            where TT : StaticComponent<TMonolith>, T
            => Locator.Provide<T, TT>(service);
        
        protected static bool Send<T>(Action<T> action) where T : StaticComponent<TMonolith>
        {
            var component = Get<T>();
            
            if (component != null)
                Receive(component, action);
            else
                StoredCommands.Add(
                    new KeyValuePair<Type, Action<StaticComponent<TMonolith>>>(
                        typeof(T), component => action?.Invoke((T)component))
                );
            
            return component != null;
        }

        private static void Receive<T>(T component, Action<T> action)
            => action?.Invoke(component);
    }
}