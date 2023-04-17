using UnityEngine;

namespace Features.Items.Weapon
{
    public class Weapon : Item<Weapon>
    {
        [SerializeField] private WeaponData_SO weaponData;

        public WeaponData_SO WeaponData => weaponData;
    }
}