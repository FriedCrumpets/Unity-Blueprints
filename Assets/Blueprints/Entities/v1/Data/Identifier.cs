using System;
using Attributes;
using UnityEngine;

namespace Blueprints.Entities
{
    [Serializable]
    public class Identifier : ISerializationCallbackReceiver
    {
        private Guid _id;
        
        public Identifier() { ID = Guid.NewGuid().ToString(); }
        
        public Identifier(string id)
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