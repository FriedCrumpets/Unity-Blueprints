using System;
using System.Collections.Generic;
using System.Linq;
using Blueprints.ServiceLocator;
using Blueprints.Utility;

namespace Blueprints.Components
{
    public class Director : IService
    {
        private static Locator _locator;

        public Director()
        {
            foreach (var pair in StoredCommands.Where(pair => pair.Key == GetType()))
            {
                Receive(this, pair.Value);
                StoredCommands.Remove(pair);
            }
        }
        
        private static Locator Locator
            => _locator ??= new Locator();

        private static List<KeyValuePair<Type, Action<Director>>> StoredCommands { get; }

        public static bool SendMessage<T>(Action<T> action) where T : Director
        {
            var director = Get<T>();
            
            if (director != null)
                Receive(director, action);
            else
                StoredCommands.Add(
                    new KeyValuePair<Type, Action<Director>>(
                        typeof(T), director => action?.Invoke((T)director)));
            
            return director != null;
        }

        private static T Get<T>() where T : IService
            => Locator.Get<T>();
        
        private static void Receive<T>(T director, Action<T> message) where T : Director
            => message?.Invoke(director); 
    }
}