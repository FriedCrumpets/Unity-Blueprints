using UnityEngine;

namespace Blueprints.Pooling.CustomPool
{
    public sealed class PooledObject : MonoBehaviour
    {
        public ObjectPool Owner { get; set; }
    }
}