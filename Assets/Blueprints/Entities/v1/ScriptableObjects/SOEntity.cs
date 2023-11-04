using UnityEngine;

namespace Blueprints.Entities
{
    [CreateAssetMenu(fileName = "Entity", menuName = "Entities/Entity", order = 0)]
    public class SOEntity : ScriptableObject
    {
        [field: SerializeField] public Identifier ID { get; private set; }
    }
}