using UnityEngine;

namespace Features.Items.Weapon
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Blueprints/Items/Weapon", order = 0)]
    public class WeaponData_SO : ScriptableObject
    {
        [SerializeField] private WeaponData data;
        
        public WeaponData Data => data;
    }

    public static class WeaponDataPrefs
    {
        public static void Save(this WeaponData_SO data)
        {
            var weaponData = data.Data;

            // PlayerPrefs.SetFloat($"{data.name} {nameof(weaponData.WeaponDeterioration)}",
            //     weaponData.WeaponDeterioration.Value);
        }

        public static void Load(this WeaponData_SO data)
        {
            var weaponData = data.Data;
            
            // weaponData.WeaponDeterioration.Value =
            //     PlayerPrefs.GetFloat($"{data.name} {nameof(weaponData.WeaponDeterioration)}",
            //         weaponData.WeaponDeterioration.Value);
        }
    }
}