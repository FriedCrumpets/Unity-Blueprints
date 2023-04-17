using System;
using UnityEngine;

namespace Blueprints.Facade
{
    [Serializable]
    public class Setting<T>
    {
        public Setting(T value, bool overrideValueCheck = false)
        {
            this.value = value;
            this.overrideCheck = overrideValueCheck;
        }

        public event Action<T> OnValueChanged;
        
        [SerializeField] protected T value;

        private bool overrideCheck;
        
        public T Value
        {
            get => value;
            set
            {
                if (!overrideCheck && value.Equals(this.value))
                {
                    return;
                }
                
                this.value = value;
                OnValueChanged?.Invoke(value);
            }
        }

        public void Destroy()
        {
            foreach (var action in OnValueChanged?.GetInvocationList()!)
            {
                OnValueChanged -= (Action<T>)action;
            }
        }
    }
}