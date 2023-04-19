using System;
using System.Collections.Generic;
using System.Linq;
using Blueprints.ServiceLocator;

namespace Blueprints.Components
{
    public abstract class Component : IService
    {
        public static event Action<Type> OnComponentCreated;
        
        private static Locator _locator;
        private static  List<KeyValuePair<Type, Action<Component>>> _storedCommands;

        public Component()
        {
            Locator.Provide(this);    
            OnComponentCreated?.Invoke(GetType());

            foreach (var pair in StoredCommands.Where(pair => pair.Key == GetType()))
            {
                Receive(this, pair.Value);
                StoredCommands.Remove(pair);
            }
        }
        
        private static Locator Locator 
            => _locator ??= new();
        
        private static List<KeyValuePair<Type, Action<Component>>> StoredCommands 
            => _storedCommands ??= new();

        protected static T Get<T>() where T : Component
            => Locator.Get<T>();

        protected static bool Send<T>(Action<T> action) where T : Component
        {
            var component = Get<T>();
            
            if (component != null)
                Receive(component, action);
            else
                StoredCommands.Add(
                    new KeyValuePair<Type, Action<Component>>(typeof(T), component => action?.Invoke((T)component)));
            
            return component != null;
        }

        private static void Provide<T>(T component) where T : Component
            => Locator.Provide(component);
        
        private static void Receive<T>(T component, Action<T> action)
            => action?.Invoke(component);
    }
}