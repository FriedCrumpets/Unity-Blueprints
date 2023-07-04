using System;
using System.Collections.Generic;
using Blueprints.Utility;

namespace Blueprints.Components
{
    public class Director : IService
    {
        private static event Action<IService> DirectorAdded;
        private static Locator Directors { get; }
        private static List<KeyValuePair<Type, Action<IService>>> StoredCommands { get; }

        static Director()
        {
            Directors = new Locator();
            StoredCommands = new List<KeyValuePair<Type, Action<IService>>>();
            DirectorAdded += service =>
            {
                foreach (var pair in StoredCommands)
                {
                    if (pair.Key == service.GetType())
                    {
                        Receive(service, pair.Value);
                        StoredCommands.Remove(pair);
                    }
                }
            };
        }
        
        public static T Get<T>() where T : IService
            => Directors.Get<T>();

        public static bool TryGet<T>(out T value) where T : IService
            => Directors.TryGet(out value);

        public static T Add<T>(T service) where T : IService
        {
            DirectorAdded?.Invoke(service);
            return Directors.Add(service);
        }

        public static bool Remove<T>(T service) where T : IService
            => Directors.Remove<T>();

        public static bool SendMessage<T>(Action<T> action) where T : IService
        {
            var component = Get<T>();
            
            if (component != null)
                Receive(component, action);
            else
                StoredCommands.Add(
                    new KeyValuePair<Type, Action<IService>>(
                        typeof(T), service => action?.Invoke((T)service)));
            
            return component != null;
        }
        
        private static void Receive<T>(T component, Action<T> message) where T : IService
            => message?.Invoke(component); 
    }
}