using System;
using System.Collections.Generic;
using System.Linq;
using Blueprints.Utility;
using UnityEngine;

namespace Blueprints.ServiceLocator
{
    public sealed class Locator : IDisposable
    {
        public event Action<IService> OnServiceProvided;

        private readonly Dictionary<Type, IService> _services;

        public Locator()
        {
            _services = new Dictionary<Type, IService>();
        }

        public List<IService> All()
            => _services.Select(pair => pair.Value).ToList();
        
        public T Get<T>() where T : IService
        {
            if (_services.ContainsKey(typeof(T)))
            {
                return (T)_services[typeof(T)];
            }
            
            Debug.LogWarning($"Service {typeof(T)} has not yet been provided to Service");
            Debug.LogWarning($"Returning Null Service {typeof(T)}");
            
            return default;
        }

        public bool TryGet<T>(out T value) where T : IService
        {
            value = Get<T>();
            return value == null;
        }

        public void SaveAll()
        {
            foreach (var service in _services)
            {
                if (service.Value is not ILoadable loadable)
                {
                    continue;
                }

                loadable.Save();
                Debug.Log($"{service.Value} saved to PlayerPrefs");
            }
        }
        
        public bool Remove<T>(bool save = true) where T : IService
        {
            if (save && _services[typeof(T)] != null)
            {
                if (_services[typeof(T)] is ILoadable loadable)
                {
                    loadable.Save();
                    Debug.Log($"Service {typeof(T)} saved");
                }
            }

            var success = _services.Remove(typeof(T));

            Debug.LogWarning(success
                ? $"Service {typeof(T)} removed from {nameof(Locator)}"
                : $"Attempting to remove a Service: {typeof(T)} that does not exist in {nameof(Locator)}");

            return success;
        }

        public T Provide<T>(T service) where T : IService
            => Provide<T, T>(service);
        
        public T2 Provide<T1, T2>(T2 service)
            where T1 : IService
            where T2 : T1
        {
            if (_services.ContainsKey(typeof(T1)))
            {
                Debug.Log($"{nameof(Locator)} already contains a service for {typeof(T1)}" +
                          $"\r\nCannot Overwrite Existing Services");
                return (T2)_services[typeof(T1)];
            }
            
            _services.Add(typeof(T1), service);
            OnServiceProvided?.Invoke(service);

            if (service is ILoadable loadable)
            {
                loadable.Load();
                Debug.Log($"{service} Loaded");
            }
            
            Debug.Log($"Service {typeof(T1)} : {service} added to Services");
            return service;
        }
        
        public void Dispose()
        {
            SaveAll();

            foreach (var service in _services)
            {
                if (service.Value is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            
            _services.Clear();
        }
    }
}