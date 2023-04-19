using System;
using System.Collections.Generic;
using UnityEngine;

namespace Blueprints.ServiceLocator
{
    public interface IService { }
    
    public interface ILoadable : IService
    {
        void Save();
        void Load();
    }
    
    public sealed class Locator : IDisposable
    {
        private Dictionary<Type, IService> _services;
        private Dictionary<Type, IService> Services => _services ??= new();

        public event Action<IService> OnServiceProvided;

        public T Get<T>() where T : IService
        {
            if (Services.ContainsKey(typeof(T)))
            {
                return (T)Services[typeof(T)];
            }
            
            Debug.LogWarning($"Service {typeof(T)} has not yet been provided to Service");
            Debug.LogWarning($"Returning Null Service {typeof(T)}");
            
            return default;
        }

        public void SaveAll()
        {
            foreach (var service in Services)
            {
                if (service.Value is not ILoadable loadable)
                {
                    continue;
                }

                loadable.Save();
                Debug.Log($"{service.Value} saved to PlayerPrefs");
            }
        }
        
        public void Remove<T>(bool save = true) where T : IService
        {
            if (save && Services[typeof(T)] != null)
            {
                if (Services[typeof(T)] is ILoadable loadable)
                {
                    loadable.Save();
                    Debug.Log($"Service {typeof(T)} saved");
                }
            }

            Debug.LogWarning(Services.Remove(typeof(T)) 
                ? $"Service {typeof(T)} removed from {nameof(Locator)}"
                : $"Attempting to remove a Service: {typeof(T)} that does not exist in {nameof(Locator)}");
        }

        public void Provide<T>(T service) where T : IService
            => Provide<T, T>(service);
        
        public void Provide<T1, T2>(T2 service)
            where T1 : IService
            where T2 : T1
        {
            if (Services.ContainsKey(typeof(T1)))
            {
                Debug.Log($"{nameof(Locator)} already contains a service for {typeof(T1)}" +
                          $"\r\nCannot Overwrite Existing Services");
                return;
            }
            
            Services.Add(typeof(T1), service);
            OnServiceProvided?.Invoke(service);

            if (service is ILoadable loadable)
            {
                loadable.Load();
                Debug.Log($"{service} Loaded");
            }
            
            Debug.Log($"Service {typeof(T1)} : {service} added to Services");
        }
        
        public void Dispose()
        {
            SaveAll();

            foreach (var service in Services)
            {
                if (service.Value is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            
            Services.Clear();
        }
    }
}