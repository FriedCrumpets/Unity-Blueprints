using System;
using UnityEngine;

namespace Blueprints.SystemFactory
{
    [Serializable]
    public class Entity
    {
        [field: SerializeField] private UnityID ID { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public string Type { get; set; }
        [field: SerializeField] public string AssetAddress { get; set; }
        public Guid[] Systems { get; set; }
    }
}