using UnityEngine;

namespace Blueprints.Singleton
{
    public static partial class Utility
    {
        public static Transform ForceRootObject(string name)
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
    
    public abstract class Singleton<T> : MonoBehaviour where T : UnityEngine.Component
    {
        private static T _instance;

        [field: SerializeField] public bool PersistBetweenScenes { get; set; }
        
        public static T Instance
        {
            get => _instance;
            private set
            {
                if (_instance == null)
                {
                    _instance = value;
                }
            }
        }

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            
            Instance = GetComponent<T>();

            if (PersistBetweenScenes)
            {
                DontDestroyOnLoad(this);
            }
        }
        
        private void OnDestroy() => DestroyInstance();
        
        public static void CreateInstance()
        {
            Instance = (T) FindObjectOfType(typeof(T), true);

            if (Instance == null)
            {
                var root = Utility.ForceRootObject("Singletons");
                var go = new GameObject(typeof(T).Name);
                go.transform.SetParent(root, false);
                Instance = go.AddComponent<T>();
            }
        }
        
        public static void DestroyInstance()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
                Instance = null;
            }
        }
    }
}