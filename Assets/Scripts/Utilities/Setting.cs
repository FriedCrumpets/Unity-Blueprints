using System;
using UnityEngine;

namespace Core.Utils
{
    [Serializable]
    public class Setting<T> : IDisposable
    {
        public event Action<T> OnValueChanged;
        
        [SerializeField] protected T value;
        
        public Setting()
        {
            value = default;
        }

        public Setting(T value)
        {
            this.value = value;
        }
        
        public virtual T Value
        {
            get => value;
            set
            {
                if (!this.value.Equals(value))
                {
                    OnValueChanged?.Invoke(value);
                    this.value = value;
                }
            }
        }

        public void Refire()
            => OnValueChanged?.Invoke(value);
        
        public void Dispose()
        {
            var actions = OnValueChanged?.GetInvocationList();

            if (actions == null)
            {
                return;
            }
            
            foreach (var action in actions)
            {
                OnValueChanged -= (Action<T>)action;
            }
        }
    
    }
}