using System;
using System.Collections.Generic;
using Blueprints.Items;
using UnityEngine;

namespace Blueprints.Inventory
{
    [Serializable]
    public class Inventory<T> where T : Item, ICollectable<T>
    {
        public Inventory() 
        {
            Items = new List<T>();
        }

        public Inventory(uint capacity) : this()
        {
            Capacity = capacity;
        }

        public Inventory(IEnumerable<ICollectable<T>> items) : this()
        {
            foreach (var item in items)
            {
                if (TryCollectItem(item) == false)
                {
                    item.Discard();
                }
            }
        }

        public Inventory(IEnumerable<ICollectable<T>> items, uint capacity) : this(capacity)
        {
            foreach (var item in items)
            {
                if (TryCollectItem(item) == false)
                {
                    item.Discard();
                };
            }
        }

        public event Action<T> OnItemCollected;
        public event Action OnInventoryFull;
        
        [field: SerializeField] public uint Capacity { get; private set; } = 28;
        [field: SerializeField] public List<T> Items { get; set; }
        
        // If inventory is full but item is stackable?!
        /*
         * Order of execution
         * if(item is stackable) { StackHasCapacity { add to stack } else { try add to inventory } }
         * { invoke inventory full } else { add to inventory } } }
         */
        public bool TryCollectItem(ICollectable<T> collectable)
        {
            Func<ICollectable<T>, bool> action = collectable.IsStackable switch
            {
                true => TryStackItem,
                false => TryAddToInventory
            };

            return action.Invoke(collectable);
        }
        
        private bool TryStackItem(IStackable<T> stackable)
        {
            var item = Items.Find(item => item.GetType() == stackable.GetType());

            if (CanStackItem(item) == false)
            {
                return TryAddToInventory(stackable.Item);
            }
            
            item.Items.Add(stackable.Item);
            OnItemCollected?.Invoke(stackable.Item);
            return true;
        }

        private bool TryAddToInventory(ICollectable<T> collectable)
        {
            if (IsInventoryFull())
            {
                OnInventoryFull?.Invoke();
                return false;
            }
            
            var item = collectable.Collect();
            Items.Add(item);
            OnItemCollected?.Invoke(item);
            return true;
        }

        public bool IsInventoryFull() => Items.Count < Capacity;

        public bool CanStackItem(IStackable<T> stackable) => stackable.Items.Count < stackable.Capacity;
    }
    
    public interface ICollectable<T> : IStackable<T> where T : Item
    {
        T Collect();
        T Discard();
        
        void Collect(Item item);
        void Discard(Item item);
    }
    
    
    public interface IStackable<T> where T : Item
    {
        T Item { get; }
        
        bool IsStackable { get; set; }
        
        uint Capacity { get; set; }
        
        List<T> Items { get; set; }
    }

    
    /*
     * Basic Implementation would be
     *      public Inventory = new Inventory<Item>();
     */
}