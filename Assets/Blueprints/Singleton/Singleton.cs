using UnityEngine;

namespace Blueprints.Singleton
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        private static Singleton<T> _instance;

        [field: SerializeField] public bool PersistBetweenScenes { get; set; }
        
        public static Singleton<T> Instance
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
            
            if(PersistBetweenScenes) { DontDestroyOnLoad(this); }
        }
    }
}