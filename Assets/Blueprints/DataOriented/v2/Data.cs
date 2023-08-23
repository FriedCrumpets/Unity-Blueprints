using System;
using UnityEngine;

namespace Blueprints.DoD.v2
{

    public interface IData<T>
    {
        T Get();
        void Set(T value);
    }
    
    [Serializable]
    public struct Data<T> : IData<T>
    {
        public Data(T value = default)
        {
            Value = value;
        }
        
        [field: SerializeField] public T Value { get; private set; }

        T IData<T>.Get()
            => Value;

        void IData<T>.Set(T value)
        {
            if (Value.Equals(value)) 
                return;
            
            Value = value;
        }
    }
}