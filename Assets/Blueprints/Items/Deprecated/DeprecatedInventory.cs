using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Blueprints.Items
{
    [Serializable][Obsolete]
    public class DeprecatedInventory<T> where T : Item<T>, DeprecatedICollectable<T>
    {
        public DeprecatedInventory() 
        {
            Items = new List<T>();
        }

        public DeprecatedInventory(uint capacity) : this()
        {
            Capacity = capacity;
        }

        public DeprecatedInventory(IEnumerable<DeprecatedICollectable<T>> items) : this()
        {
            foreach (var item in items)
            {
                if (TryCollectItem(item) == false)
                {
                    item.Discard();
                }
            }
        }

        public event Action<T> OnItemCollected;
        public event Action OnInventoryFull;
        
        [field: SerializeField] public uint Capacity { get; private set; } = 28;
        [field: SerializeField] public List<T> Items { get; set; }
        
        public bool TryCollectItem(DeprecatedICollectable<T> deprecatedICollectable)
        {
            // return deprecatedICollectable is DeprecatedIStackable<T> == null ? 
            //         TryStackItem(deprecatedICollectable as DeprecatedIStackable<T>) : 
            //         TryAddToInventory(deprecatedICollectable); 
            return false;
        }

        public bool IsInventoryFull() => Items.Count >= Capacity;

        public bool CanStackItem(DeprecatedIStackable<T> deprecatedIStackable) => 
            deprecatedIStackable.Items.Count < deprecatedIStackable.Capacity;

        public bool DiscardItem(DeprecatedICollectable<T> deprecatedICollectable)
        {
            var discard = Items.Find(item => item == (T)deprecatedICollectable);
            if (!Items.Remove(discard))
            {
                return false;
            }
            
            discard.Discard();
            return true;
        }
        
        private bool TryStackItem(DeprecatedIStackable<T> deprecatedIStackable)
        {
            var item = Items.FirstOrDefault(item => item.GetType() == deprecatedIStackable.GetType()) as DeprecatedIStackable<T>;

            if (item == null || CanStackItem(item) == false)
            {
                return TryAddToInventory(deprecatedIStackable.Item);
            }
            
            item.Items.Add(deprecatedIStackable.Item);
            OnItemCollected?.Invoke(deprecatedIStackable.Item);
            return true;
        }

        private bool TryAddToInventory(DeprecatedICollectable<T> deprecatedICollectable)
        {
            if (IsInventoryFull())
            {
                OnInventoryFull?.Invoke();
                return false;
            }
            
            var item = deprecatedICollectable.Collect();
            // item.Options.ForEach(option => Debug.Log(option.DisplayName));
            Items.Add(item);
            OnItemCollected?.Invoke(item);
            return true;
        }
    }
    
    [Obsolete]
    public interface DeprecatedICollectable<out T> where T : Item<T>
    {
        T Collect();
        T Discard();
    }
    
    [Obsolete]
    public interface DeprecatedIStackable<T> where T : Item<T>
    {
        T Item { get; }

        uint Capacity { get; set; }
        
        List<T> Items { get; set; }
    }

    /*
     * Basic Implementation would be
     *      public Inventory = new Inventory<Item>();
     */
}