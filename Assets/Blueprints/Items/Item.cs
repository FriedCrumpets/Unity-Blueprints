using System;
using System.Collections.Generic;
using System.Linq;
using Blueprints.Command;
using UnityEngine;

namespace Blueprints.Items
{
    [Serializable]
    public abstract class Item : MonoBehaviour
    {
        public List<Option<Item>> Options { get; set; }

        public string Name { get; set; }
        
        /* TODO:
            Require moving to some kind of Properties container
            this way Items can morph into Skills and other stats with options
         
            public int Weight { get; set; }
        */

        public void AddOption(Option<Item> option) => Options.Add(option);
        public bool RemoveOption(Option<Item> option) => Options.Remove(option);

        public bool RemoveOption(string optionName)
        {
            var first = Options.FirstOrDefault(option => option.DisplayName.Contains(optionName));
            return Options.Remove(first);
        }
    }

    public interface IDroppable
    {
        void Drop(Item item);
    }

    public interface IDestroyable
    {
        void Destroy(Item item);
    }
    
    public interface IUsable
    {
        bool Used { get; set; }

        void Use(Item item);
    }

    public interface IEquippable
    {
        bool Equipped { get; set; }

        void Equip(Item item);
        void UnEquip(Item item);
    }

    public interface IExamineable
    {
        bool CanExamine { get; set; }

        void Examine();
    }

    /// <summary>
    /// Provides the ability to reclaim an item upon death. Similar mechanic implemented by Runescape
    /// </summary>
    public interface IReclaimable
    {
        bool CanReclaim { get; set; }
    }
    
    public interface ITradeable  
    {
        IEnumerable<Item> Value { get; set; }
        
        /*
         I NEED a lot more information on this before I can figure this out. 
         Trading is an entire system separate to Items
         // TODO: Investigate trading systems within games to discover mechanics
         
         Tradeable items aren't always traded with coin, coins/items... are coins an item
        
         Items can be traded with ITradeable, but the vendor supplies the logic to buy and sell the item.
         
         I Think the cost for an item is better reserved for a Cost lookup table. 
         Similar to a drop table In a way. This can be an API styled json file that's received containing cost information.
         */
    }

    // Interaction extension interfaces should become available also
}