using UnityEngine;
using UnityEngine.Pool;

namespace Blueprints.Pool
{
    public abstract class ComponentPool<T> where T : Component
    {
        protected ComponentPool(int maxPoolSize, int defaultCapacity)
        {
            MaxPoolSize = maxPoolSize;
            DefaultCapacity = defaultCapacity;
        }

        [field: SerializeField] public int MaxPoolSize { get; private set; }
        [field: SerializeField] public int DefaultCapacity { get; private set; }

        private IObjectPool<T> _pool = null;
        
        public IObjectPool<T> Pool =>
            _pool ??= new ObjectPool<T>(
            CreateItem,
            OnRetrieveFromPool,
            OnReturnToPool,
            OnDestroyPoolObject,
            true,
            DefaultCapacity,
            MaxPoolSize);

        public abstract void Spawn();
        
        protected abstract T CreateItem();

        protected abstract void OnRetrieveFromPool(T item);

        protected abstract void OnReturnToPool(T item);

        protected abstract void OnDestroyPoolObject(T item);
    }
}