using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.Player.Consumable
{
    [CreateAssetMenu(fileName = "ConsumableData", menuName = "ScriptableObjects/Player/Data/Consumable", order = 0)]
    public class ConsumableData_SO : ScriptableObject
    {
        [SerializeField] private List<PlayerStatEffect> effects;

        public override string ToString()
            => effects.Aggregate(string.Empty, (current, effect) => current + effect);
    }
}