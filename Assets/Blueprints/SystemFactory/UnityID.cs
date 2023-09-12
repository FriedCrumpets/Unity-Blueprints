using System;
using Attributes;
using UnityEngine;

namespace Blueprints.SystemFactory
{
    [Serializable]
    public class UnityID : ISerializationCallbackReceiver
    {
        private Guid _id;
        
        public UnityID() { ID = Guid.NewGuid().ToString(); }
        
        public UnityID(string id)
        {
            ID = id;
            _id = Guid.Parse(id);
        }
        
        [field: SerializeField, ReadOnly] public string ID { get; set; }
        
        public void OnBeforeSerialize()
            => _id = Guid.Parse(ID);

        public void OnAfterDeserialize()
            => ID = _id.ToString();

        [ContextMenu("ReGenerateID")]
        private void ReGenerateID()
            => ID = Guid.NewGuid().ToString();
    }
}