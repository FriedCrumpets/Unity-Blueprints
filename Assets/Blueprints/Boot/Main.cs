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
        public UnityEvent bootUpComplete;
        
        private readonly GameObject _bootObject;

        public Main(GameObject bootObject)
        {
            _bootObject = bootObject;
            Loaded = new();
        }
        
        [field: SerializeField] public List<AssetReference> Loaders { get; set; }
        
        private List<GameObject> Loaded { get; }

        public void Boot()
        {
            InstantiateLoaders(Loaders, () =>
            {
                bootUpComplete?.Invoke(); _bootObject.name = GetType().Name;
            });
        }

        private void InstantiateLoaders(IList<AssetReference> loaders, Action onComplete = null)
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