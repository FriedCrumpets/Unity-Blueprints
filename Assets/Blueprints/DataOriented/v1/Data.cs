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
        
        public Data(T value = default)
        {
            Value = value;
        }
        
        [field: SerializeField] public T Value { get; private set; }

        Action<T> IData<T>.Notifier
        {
            get => Notifier;
            set => Notifier += value;
        }
        
        T IData<T>.Get()
            => Value;

        void IData<T>.Set(T value)
        {
            if (Value.Equals(value)) 
                return;
            
            Notifier?.Invoke(value);
            Value = value;
        }
    }
}