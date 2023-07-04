using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using static UnityEngine.Object;

namespace Blueprints.Boot
{
    [Serializable]
    public class Main : IDisposable
    {
        public event Action Complete;
        
        public UnityEvent bootUpComplete;
        
        private GameObject _bootObject;

        public Main()
        {
            Loaded = new List<GameObject>();
            Loaders = new List<AssetReference>();
            Complete += () => bootUpComplete?.Invoke();
        }
        
        [field: SerializeField] public List<AssetReference> Loaders { get; set; }
        
        private List<GameObject> Loaded { get; }

        public void Boot(GameObject bootObject)
        {
            _bootObject = bootObject;
            
            InstantiateLoaders(Loaders, () =>
            {
                Complete?.Invoke(); _bootObject.name = GetType().Name;
            });
        }

        private void InstantiateLoaders(ICollection<AssetReference> loaders, Action onComplete = null)
        {
            var cAction = new CountAction(loaders.Count, onComplete);
            
            foreach (var loader in loaders)
            {
                var loaded = Addressables.InstantiateAsync(loader, _bootObject.transform, true);
                loaded.Completed += (handle) =>
                {
                    handle.Result.name = handle.Result.name.Replace("(Clone)", "");
                    Loaded.Add(handle.Result);
                    cAction.Decrement();
                };
            }
        }

        public void Dispose()
        {
            foreach (var loader in Loaded)
            {
                Destroy(loader);
            }
        }
    }
}