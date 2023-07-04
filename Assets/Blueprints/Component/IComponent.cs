using System;
using System.Collections.Generic;
using Blueprints.Utility;

namespace Blueprints.Components
{
    public interface IComponent : IService
    {
        bool AutomaticMigration { get; }
        IComponent Master { get; set; }
        Action<IComponent> ComponentCreated { get; }
        Locator Components { get; }
        List<KeyValuePair<Type, Action<IService>>> StoredCommands { get; }

        T Get<T>() where T : IService;
        TValue Get<TKey, TValue>() where TKey : IService;
        bool TryGet<T>(out T value) where T : IService;
        T Add<T>(T service) where T : IService;
        bool Remove<T>(T service) where T : IService;

        void AddComponent<T>(T component) where T : IComponent;
        void AddComponent<T>(Type type, T component) where T : IComponent;
        
        static bool SendMessage<T>(IComponent master, Action<T> action) where T : IService
        {
            var component = master.Get<T>();
            
            if (component != null)
                Receive(component, action);
            else
                master.StoredCommands.Add(
                    new KeyValuePair<Type, Action<IService>>(
                        typeof(T), component => action?.Invoke((T)component)));
            
            return component != null;
        }
        
        static void Receive<T>(T component, Action<T> message) where T : IService
            => message?.Invoke(component);
    }
}