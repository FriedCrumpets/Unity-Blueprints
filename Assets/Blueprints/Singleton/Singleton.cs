using UnityEngine;

namespace Blueprints.Singleton
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        private static MonoSingleton<T> _instance;

        [field: SerializeField] public bool DontDestroyOnLoad { get; set; }
        
        public static MonoSingleton<T> Instance
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
            
            Instance = this;
        }
    }
}