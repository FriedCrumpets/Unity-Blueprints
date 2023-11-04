using UnityEngine;

namespace Blueprints.Entities
{
    [CreateAssetMenu(fileName = "Actor", menuName = "Entities/Actor", order = 0)]
    public class SOActor : SOEntity
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public EEntityType Type { get; private set; }
    }
}