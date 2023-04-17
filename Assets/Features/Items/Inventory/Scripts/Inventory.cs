using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.Items.Inventory
{
    [Serializable]
    public class Inventory
    {
        /// <summary>
        /// Creating an Inventory with a null type allows for all types to be contained within
        /// </summary>
        /// <param name="inventoryType"></param>
        private Inventory()
        {
            items = new List<ItemData>();
        }

        public Inventory(int capacity) : this()
        {
            this.capacity = capacity;
        }

        public Inventory(IEnumerable<ItemData> items, int capacity) : this(capacity)
        {
            foreach (var item in items)
            {
                this.items.Add(item);
            }
        }

        public event Action<ItemData> OnItemCollected;
        public event Action<ItemData> OnItemFailedCollection;
        public event Action OnInventoryFull;

        [SerializeField] private List<ItemData> items;
        [SerializeField] private int capacity;

        public List<Type> InventoryTypeWhitelist { get; } = new();
        public List<Type> InventoryTypeBlacklist { get; } = new();

        public bool IsFull
        {
            get
            {
                var full = items.Count >= capacity && capacity < 0;
                if (full)
                    OnInventoryFull?.Invoke();

                return full;
            }
        }

        public bool TryCollectItem(ItemData item)
        {
            // todo : try to stack the item, if the item cannot be stacked, TryToCollect it
            if (IsFull)
            {
                return CollectionSuccessful(() => TryStackItem(item), item);
            }
            
            /*
             * if white list is empty
             */
            
            if (CollectionSuccessful(()
                => !InventoryTypeWhitelist.Any() || 
                   (InventoryTypeWhitelist.Contains(item.ItemType)  
                   && !InventoryTypeBlacklist.Contains(item.ItemType)) , item))
            {
                CollectItem(item);
                return true;
            }

            return false;
        }

        private bool CollectionSuccessful(Func<bool> successMetric, ItemData item)
        {
            if (successMetric.Invoke())
            {
                OnItemCollected?.Invoke(item);
                return true;
            }
            
            OnItemFailedCollection?.Invoke(item);
            return false;

        }
        
        private void CollectItem(ItemData collectedItem)
        {

        }

        private bool TryStackItem(ItemData collectedItem)
        {
            /*
             * todo: If item is stackable look for existing item and add collected item stack to it.
             */

            if (ItemExists(collectedItem.ID))
            {
                
            }
            
            return false;
        }

        private void StackItem(ItemData data)
        {
            
        }

        private bool ItemExists(uint id)
            => items.Any(item => item.ID == id);
    }
}