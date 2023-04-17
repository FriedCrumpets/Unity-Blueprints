using System;
using UnityEngine;

namespace Features.Items
{
    [Serializable]
    public struct ItemData
    {
        [SerializeField] private uint id;
        [SerializeField] private string name;
        [SerializeField] private Sprite sprite;
        [SerializeField] private GameObject gameObject;
        [SerializeField] private string description;
        [SerializeField] private float weight;
        [SerializeField] private Stack stack;
        
        private Type itemType;
        
        public uint ID => id;
        public string Name => name;
        public Sprite Sprite => sprite;
        public GameObject GameObject => gameObject;
        public string Description => description;
        public float Weight => weight;
        public Stack Stack => stack;

        public Type ItemType
        {
            get => itemType;
            set => itemType = value;
        }
    }

    [Serializable]
    public class Stack
    {
        [SerializeField] private int maxStack;
        [SerializeField] private uint stackAmount;
        
        public int MaxStack => maxStack;
        public uint StackAmount => stackAmount;
    }
}