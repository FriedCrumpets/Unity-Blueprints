using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SubsystemsImplementation.Extensions;

namespace Blueprints.IO.Behaviours
{
    public abstract class SaveableBehaviour : MonoBehaviour,
        ISaveable,
        ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector]
        private Guid _saveId;
        
        public Guid SaveID
        {
            get => _saveId;
            set => _saveId = value;
        }
        
        public abstract JObject SavedData { get; }

        public abstract void LoadFromData(JObject data);

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            // If the _saveId doesn't exist sets one up
            if (_saveId == default)
                _saveId = Guid.NewGuid();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize() { }
    }
}