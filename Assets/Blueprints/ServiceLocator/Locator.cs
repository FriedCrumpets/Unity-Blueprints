
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Blueprints.Utility
{
    public interface IService { }
    
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

        public List<KeyValuePair<Type, IService>> AllPairs()
            => _services.Select(pair => pair).ToList();

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
        
        public TValue Get<TKey, TValue>()
        {
            if (_services.ContainsKey(typeof(TKey)))
            {
                return (TValue)_services[typeof(TKey)];
            }
            
            Debug.LogWarning($"Service {typeof(TKey)} has not yet been provided to Locator");
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

        public bool TryGet<T>(out T value) where T : IService
        {
            value = Get<T>();
            return value == null;
        }
        
        public bool TryGet<TKey, TValue>(out TValue value) where TValue : IService
        {
            value = Get<TKey, TValue>();
            return value == null;
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
        
        public T Add<T>(T service) where T : IService
            => Add<T, T>(service);

        public TValue Add<TValue>(Type type, TValue service) where TValue : IService
        {
            if (_services.ContainsKey(type))
            {
                Debug.Log($"{nameof(Locator)} already contains a service for {type}" +
                          $"\r\nCannot Overwrite Existing Services");
                return (TValue)_services[type];
            }
            
            _services.Add(type, service);
            OnServiceProvided?.Invoke(service);

            Debug.Log($"Service {type} : {service} added to Services");
            return service;
        }
        
        public TValue Add<TKey, TValue>(TValue service)
            where TValue : IService
        {
            if (_services.ContainsKey(typeof(TKey)))
            {
                Debug.Log($"{nameof(Locator)} already contains a service for {typeof(TKey)}" +
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
    
    public static class Locator<T> 
    {
        public static event Action<object> OnServiceProvided;

        private static readonly Dictionary<Type, object> _services;

        static Locator()
        {
            _services = new Dictionary<Type, object>();
        }

        public static List<object> All
            => _services.Select(pair => pair.Value).ToList();
        
        public static T1 Get<T1>()
        {
            if (_services.ContainsKey(typeof(T1)))
            {
                return (T1)_services[typeof(T1)];
            }
            
            Debug.LogWarning($"Service {typeof(T1)} has not yet been provided to Service");
            Debug.LogWarning($"Returning Null Service {typeof(T1)}");
            
            return default;
        }
        
        public static TValue Get<TKey, TValue>()
        {
            if (_services.ContainsKey(typeof(TKey)))
            {
                return (TValue)_services[typeof(TKey)];
            }
            
            Debug.LogWarning($"Service {typeof(TKey)} has not yet been provided to Locator");
            Debug.LogWarning($"Returning Null Service {typeof(TKey)}");
            
            return default;
        }
        
        public static T1 Retrieve<T1>()
        {
            var service = Get<T1, Func<T1>>();
            return service == default ? default : service();
        }
        
        public static TValue Retrieve<TKey, TValue>()
        {
            var service = Get<TKey, Func<TValue>>();
            return service == default ? default : service();
        }
        
        public static T1 Add<T1>(T1 service)
            => Add<T1, T1>(service);
        
        public static TValue Add<TKey, TValue>(TValue service)
            where TValue : TKey
        {
            if (_services.ContainsKey(typeof(TKey)))
            {
                Debug.Log($"{nameof(Locator<T>)} already contains a service for {typeof(TKey)}" +
                          $"\r\nCannot Overwrite Existing Services");
                return (TValue)_services[typeof(TKey)];
            }
            
            _services.Add(typeof(TKey), service);
            OnServiceProvided?.Invoke(service);

            Debug.Log($"Service {typeof(TKey)} : {service} added to Services");
            return service;
        }

        public static bool Remove<T1>()
        {
            var success = _services.Remove(typeof(T1));

            Debug.LogWarning(success
                ? $"Service {typeof(T1)} removed from {nameof(Locator<T>)}"
                : $"Attempting to remove a service failed: {typeof(T1)} could not be removed from {nameof(Locator<T>)}");

            return success;
        }

        public static void Dispose()
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