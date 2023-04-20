using System;
using System.Collections.Generic;
using System.Linq;
using Blueprints.ServiceLocator;

namespace Blueprints.Components
{
    public abstract class Component : IService, IDisposable
    {
        private MonoComponent Master { get; }
        
        public Component(MonoComponent master)
        {
            master.AddService(this);    
            master.OnComponentCreated?.Invoke(GetType());

            foreach (var pair in master.StoredCommands.Where(pair => pair.Key == GetType()))
            {
                Receive(this, pair.Value);
                master.StoredCommands.Remove(pair);
            }

            Master = master;
        }

        protected static bool SendMessage<T>(MonoComponent master, Action<T> action) where T : Component
        {
            var component = master.GetService<T>();
            
            if (component != null)
                Receive(component, action);
            else
                master.StoredCommands.Add(
                    new KeyValuePair<Type, Action<Component>>(typeof(T), component => action?.Invoke((T)component)));
            
            return component != null;
        }

        private static void Receive<T>(T component, Action<T> action)
            => action?.Invoke(component);

        public void Dispose()
            => Master.RemoveService(this);
    }
}