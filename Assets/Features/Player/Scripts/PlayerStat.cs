using Blueprints.DoD;
using Blueprints.Facade;
using UnityEngine;

namespace Features.Player
{
    [System.Serializable]
    public class PlayerStat : ClampedDataSet
    {
        public PlayerStat(float value, float min, float max) : base(value, min, max) { }

        [SerializeField] private string name;

        public string Name => name;
    }
}