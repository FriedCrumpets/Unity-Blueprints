using System;
using UnityEngine;

namespace Core.Utils
{
    [Serializable]
    public class ClampedSetting : Setting<float>, IDisposable
    {
        public ClampedSetting(float value, float min, float max) : base(value)
        {
            this.min = min;
            this.max = max;
        }

        [SerializeField] private float min;
        [SerializeField] private float max;

        public new event Action<float> OnValueChanged;

        public new float Value
        {
            get => value;
            set
            {
                if (value.Equals(this.value))
                {
                    return;
                }
                
                var val = Mathf.Clamp(value, min, max);
                this.value = value;
                OnValueChanged?.Invoke(value);
            }
        }
    }
}