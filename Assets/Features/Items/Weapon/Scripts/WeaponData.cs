using Blueprints.DoD;
using Blueprints.Facade;
using UnityEngine;

namespace Features.Items.Weapon
{
    [System.Serializable]
    public class WeaponData
    {
        [SerializeField] private float weaponDamage;
        [SerializeField] private ClampedDataSet weaponDeterioration;
        [SerializeField] private Data<bool> isCraftable;
        
        public float WeaponDamage => weaponDamage;
        public ClampedDataSet WeaponDeterioration => weaponDeterioration;
        public Data<bool> IsCraftable => isCraftable;
    }
}