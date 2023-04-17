using System;
using System.Collections.Generic;
using UnityEngine;

namespace Blueprints.Items
{
    public abstract class Item<T>
    {
        public uint ID { get; set; }
        public string Name { get; set; }
        // public List<Option<T>> Options { get; set; }
        public Sprite Sprite { get; set; }
        public GameObject GameObject { get; set; }
    }

    [Serializable]
    public struct ItemData
    {
        public uint ID { get; set; }
        public string Name { get; set; }
        public Sprite Sprite { get; set; }
        public GameObject GameObject { get; set; }
    }

    [Serializable]
    public class Item
    {
        public Item(Item item)
        {
            data = item.data;
            Commands = item.Commands;
        }
        
        public ItemData data;
        public List<Command> Commands;
    }
}