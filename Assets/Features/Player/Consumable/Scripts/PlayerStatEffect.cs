using Blueprints.Visitor;
using UnityEngine;

namespace Features.Player.Consumable
{
    [System.Serializable]
    public class PlayerStatEffect : Effect<PlayerStats>
    {
        [SerializeField] private string statName;
        [SerializeField] private float effect;
        
        public string StatName => statName;
        public float Effector => effect;

        public override void Visit(IVisitable<PlayerStats> visitable)
            => visitable.Accept(this); 

        public override string ToString()
        {
            return Effector switch
            {
                >0 => $"{StatName} Increased by {Effector}\r\n",
                <0 => $"{StatName} Decreased by {Effector}\r\n",
                _ => string.Empty
            };
        }
    }
}