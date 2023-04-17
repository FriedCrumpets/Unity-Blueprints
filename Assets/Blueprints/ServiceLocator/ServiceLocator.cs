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
    
    public sealed class ServiceLocator : IDisposable
    {
        private Dictionary<Type, IService> Services { get; set; } = new();

        public event Action<IService> OnServiceProvided;

        public T Get<T>() where T : IService
        {
            Services ??= new();
            
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
                ? $"Service {typeof(T)} removed from {nameof(ServiceLocator)}"
                : $"Attempting to remove a Service: {typeof(T)} that does not exist in {nameof(ServiceLocator)}");
        }
        
        public void Provide<TInterface, TInstance>(TInstance service)
            where TInterface : IService
            where TInstance : TInterface
        {
            Services ??= new();
            
            if (Services.ContainsKey(typeof(TInterface)))
            {
                Debug.Log($"{nameof(ServiceLocator)} already contains a service for {typeof(TInterface)}" +
                          $"\r\nCannot Overwrite Existing Services");
                return;
            }
            
            Services.Add(typeof(TInterface), service);
            OnServiceProvided?.Invoke(service);

            if (service is ILoadable loadable)
            {
                loadable.Load();
                Debug.Log($"{service} Loaded");
            }
            
            Debug.Log($"Service {typeof(TInterface)} : {service} added to Services");
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