using System;
using UnityEngine;

namespace Blueprints.Inventory
{
    [Serializable]
    public abstract class Item : MonoBehaviour
    {
        string Name { get; set; }
        
        public abstract void Collect(); 
        
        public abstract void Discard();
    }

    public interface IUsable
    {
        bool Used { get; set; }

        void Use();
    }

    public interface IEquippable
    {
        bool Equipped { get; set; }

        void Equip();
        void UnEquip();
    }
}