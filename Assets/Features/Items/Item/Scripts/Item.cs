using System;
using UnityEngine;

namespace Features.Items
{
    public abstract class Item<T> : MonoBehaviour
    {
        [SerializeField] private ItemData_SO data;

        public void SetItemDataType()
        {
            var itemData = data.Data;
            itemData.ItemType = typeof(T);
        }
        
        public Type ItemType => typeof(T);
        public ItemData_SO Data => data;
    }
}