using System;
using System.Collections.Generic;
using Blueprints.ServiceLocator;
using Blueprints.Utility;

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
}