using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Blueprints.Boot
{
    public abstract class TBoot : MonoBehaviour
    {
        [field: SerializeField] protected List<GameObject> Loaders { get; set; }

        protected List<GameObject> Loaded { get; } = new();

        public UnityEvent bootUpComplete;

        public void InstantiateBoot()
        {
            var type = GetType();
            
            var boot = new GameObject();
            
            boot.name = type.Name;
            boot.AddComponent(type);
        }
        
        protected void Boot()
        {
            CreateSingletons();

            InstantiateLoaders(Loaders, () =>
            {
                bootUpComplete?.Invoke(); gameObject.name = GetType().Name;
            });
        }

        protected virtual void CreateSingletons() { }

        protected void InstantiateLoaders(IEnumerable<GameObject> loaders, Action onComplete = null)
        {
            foreach (var loader in loaders)
            {
                var loaded = Instantiate(loader, transform, true);
                loaded.name = loaded.name.Replace("(Clone)", "");
                Loaded.Add(loaded);
            }      
            
            onComplete?.Invoke();
        }
    }
}