using System;
using UnityEngine;

namespace Blueprints.DoD
{

    public interface IData<T>
    {
        Action<T> Notifier { get; set; }
        T Get();
        void Set(T value);
    }
    
    [Serializable]
    public class Data<T> : IData<T>
    {
        public event Action<T> Notifier;

        [SerializeField] private T value;

        public Data(T value = default)
        {
            this.value = value;
        }

        Action<T> IData<T>.Notifier
        {
            get => Notifier;
            set => Notifier += value;
        }
        
        T IData<T>.Get()
            => value;

        void IData<T>.Set(T value)
        {
            if (this.value.Equals(value)) 
                return;
            
            Notifier?.Invoke(value);
            this.value = value;
        }
    }
}