using Blueprints.Facade;
using Core.Utils;
using UnityEngine;

namespace Features.Items.Weapon
{
    [System.Serializable]
    public class WeaponData
    {
        [SerializeField] private float weaponDamage;
        [SerializeField] private ClampedSetting weaponDeterioration;
        [SerializeField] private Setting<bool> isCraftable;
        
        public float WeaponDamage => weaponDamage;
        public ClampedSetting WeaponDeterioration => weaponDeterioration;
        public Setting<bool> IsCraftable => isCraftable;
    }
}