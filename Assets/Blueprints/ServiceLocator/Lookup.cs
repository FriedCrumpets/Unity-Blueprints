using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Blueprints.Utility
{
    public class Lookup
    {
        public event Action<object> OnServiceProvided;

        private readonly Dictionary<Type, object> _services;

        public Lookup()
        {
            _services = new Dictionary<Type, object>();
        }

        public List<object> All()
            => _services.Select(pair => pair.Value).ToList();
        
        public T Get<T>()
        {
            if (_services.ContainsKey(typeof(T)))
            {
                return (T)_services[typeof(T)];
            }
            
            Debug.LogWarning($"Service {typeof(T)} has not yet been provided to Service");
            Debug.LogWarning($"Returning Null Service {typeof(T)}");
            
            return default;
        }
        
        public TValue Get<TKey, TValue>()
        {
            if (_services.ContainsKey(typeof(TKey)))
            {
                return (TValue)_services[typeof(TKey)];
            }
            
            Debug.LogWarning($"Service {typeof(TKey)} has not yet been provided to Lookup");
            Debug.LogWarning($"Returning Null Service {typeof(TKey)}");
            
            return default;
        }
        
        public T Retrieve<T>()
        {
            var service = Get<T, Func<T>>();
            return service == default ? default : service();
        }
        
        public TValue Retrieve<TKey, TValue>()
        {
            var service = Get<TKey, Func<TValue>>();
            return service == default ? default : service();
        }

        public bool TryGet<T>(out T value)
        {
            value = Get<T>();
            return value == null;
        }
        
        public bool TryGet<TKey, TValue>(out TValue value)
        {
            value = Get<TKey, TValue>();
            return value == null;
        }

        public bool Remove<T>(bool save = true)
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
                ? $"Service {typeof(T)} removed from {nameof(Lookup)}"
                : $"Attempting to remove a Service: {typeof(T)} that does not exist in {nameof(Lookup)}");

            return success;
        }
        
        public T Add<T>(T service) 
            => Add<T, T>(service);
        
        public TValue Add<TKey, TValue>(TValue service)
        {
            if (_services.ContainsKey(typeof(TKey)))
            {
                Debug.Log($"{nameof(Lookup)} already contains a service for {typeof(TKey)}" +
                          $"\r\nCannot Overwrite Existing Services");
                return (TValue)_services[typeof(TKey)];
            }
            
            _services.Add(typeof(TKey), service);
            OnServiceProvided?.Invoke(service);

            Debug.Log($"Service {typeof(TKey)} : {service} added to Services");
            return service;
        }

        public void Dispose()
        {
            foreach (var service in _services.Values)
            {
                if (service is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            
            _services.Clear();
        }
    }
}