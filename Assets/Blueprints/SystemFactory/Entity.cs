using System;
using UnityEngine;

namespace Blueprints.SystemFactory
{
    [Serializable]
    public class Entity
    {
        [field: SerializeField] private UnityID ID { get; set; }
        public Guid[] Systems { get; set; }
    }
}