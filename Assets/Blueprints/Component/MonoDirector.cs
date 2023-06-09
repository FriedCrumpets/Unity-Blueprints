using System;
using System.Collections.Generic;
using System.Linq;
using Blueprints.ServiceLocator;
using Blueprints.Utility;
using UnityEngine;

namespace Blueprints.Components
{
    public abstract class MonoDirector : MonoBehaviour, IService
    {
        private static Locator _locator;

        protected virtual void Awake()
        {
            OnDirectorCreated?.Invoke(this);
            
            foreach (var pair in StoredCommands.Where(pair => pair.Key == GetType()))
            {
                Receive(this, pair.Value);
                StoredCommands.Remove(pair);
            }
        }
        
        protected Action<MonoDirector> OnDirectorCreated { get; }

        protected static Locator Locator
            => _locator ??= new Locator(); 
        
        private static List<KeyValuePair<Type, Action<MonoDirector>>> StoredCommands { get; }

        protected static bool SendMessage<T>(Action<T> action) where T : MonoDirector
        {
            var director = Get<T>();
            
            if (director != null)
                Receive(director, action);
            else
                StoredCommands.Add(
                    new KeyValuePair<Type, Action<MonoDirector>>(
                        typeof(T), director => action?.Invoke((T)director)));
            
            return director != null;
        }

        protected static T Get<T>() where T : IService
            => Locator.Get<T>();

        private static void Receive<T>(T director, Action<T> message) where T : MonoDirector
            => message?.Invoke(director);
    }
}