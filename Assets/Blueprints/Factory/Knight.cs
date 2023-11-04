using Features.Items.Weapon;
using UnityEngine;

namespace Blueprints.Factory.Standard
{
    public class Knight : MonoBehaviour
    {
        [SerializeField] private EquipmentFactory equipmentFactory;

        private IWeapon _weapon;
        private IShield _shield;
        
        private void Start()
        {
            _weapon = equipmentFactory.CreateWeapon();
            _shield = equipmentFactory.CreateShield();
        }

        public void Attack() => _weapon?.Attack();
        public void Defend() => _shield?.Defend();
    }

    public class EquipmentFactory : ScriptableObject
    {
        public WeaponFactory weaponFactory;
        public ShieldFactory shieldFactory;

        public IWeapon CreateWeapon()
            => weaponFactory != null ? weaponFactory.CreateWeapon() : IWeapon.CreateDefault();
        
        public IShield CreateShield()
            => shieldFactory != null ? shieldFactory.CreateShield() : IShield.CreateDefault();
    }

    public interface IShield
    {
        void Defend();
        
        static IShield CreateDefault()
            => new Shield();
    }

    public class Shield : IShield
    {
        public void Defend() { }
    }
    
    public abstract class ShieldFactory : ScriptableObject
    {
        public abstract IShield CreateShield();
    }
    
    [CreateAssetMenu(fileName = "ShieldFactory", menuName = "ShieldFactory/Generic")]
    public class GenericShieldFactory : ShieldFactory
    {
        public override IShield CreateShield()
            => new Shield();
    }


    public interface IWeapon
    {
        void Attack();
        
        static IWeapon CreateDefault()
            => new Sword();
    }
    
    public class Sword : IWeapon
    {
        public void Attack() { } 
    }
    
    public class Bow : IWeapon
    {
        public void Attack() { } 
    }

    public abstract class WeaponFactory : ScriptableObject
    {
        public abstract IWeapon CreateWeapon();
    }

    [CreateAssetMenu(fileName = "SwordFactory", menuName = "WeaponFactory/Sword")]
    public class SwordFactory : WeaponFactory
    {
        public override IWeapon CreateWeapon()
            => new Sword();
    }
    
    [CreateAssetMenu(fileName = "BowFactory", menuName = "WeaponFactory/Bow")]
    public class BowFactory : WeaponFactory
    {
        public override IWeapon CreateWeapon()
            => new Bow();
    }
    
}