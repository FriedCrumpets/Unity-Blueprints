using UnityEngine;

namespace Blueprints.Entities
{
    public class SOInteractable : SOEntity
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] private EEntityType Type { get; set; }
    }
}