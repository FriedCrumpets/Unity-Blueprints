using Blueprints.Facade;
using Core.Utils;
using UnityEngine;

namespace Features.Player
{
    [System.Serializable]
    public class PlayerStat : ClampedSetting
    {
        public PlayerStat(float value, float min, float max) : base(value, min, max) { }

        [SerializeField] private string name;

        public string Name => name;
    }
}