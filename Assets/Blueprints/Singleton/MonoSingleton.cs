using Blueprints.Utility;
using UnityEngine;

namespace Blueprints.Core
{
    public static partial class Utility
    {
        public static Transform ForceGameObject(string name)
        {
            var root = GameObject.Find(name);
            
            if (root == null)
            {
                root = new GameObject(name)
                {
                    transform =
                    {
                        localPosition = Vector3.zero,
                        localRotation = Quaternion.identity
                    }
                };
            }
            
            return root.transform;
        }
    }
    
    public abstract class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Get()
            => _instance;

        protected virtual void Awake()
        {
            if (_instance != null)
            {
                Destroy(this);
                return;
            }
            
            _instance = GetComponent<T>();

            if (_instance is ILoadable loadable)
            {
                loadable.Load();
            }
        }
        
        private void OnDestroy() 
            => DestroyInstance();
        
        public static T CreateInstance()
        {
            _instance = (T) FindObjectOfType(typeof(T), true);

            if (_instance == null)
            {
                var root = Utility.ForceGameObject("Singletons");
                var go = new GameObject(typeof(T).Name);
                go.transform.SetParent(root, false);
                _instance = go.AddComponent<T>();
            }

            return _instance;
        }

        public static void DestroyInstance()
        {
            if (_instance != null)
            {
                if (_instance is ILoadable loadable)
                {
                    loadable.Save();
                }
                
                Destroy(_instance.gameObject);
                _instance = null;
            }
        }
    }
}