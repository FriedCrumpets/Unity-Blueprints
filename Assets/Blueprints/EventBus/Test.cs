using System;
using UnityEngine;

namespace Blueprints.EventBus
{
    public enum TestEnum
    {
        ONE,
        TWO,
        THREE
    }
    
    public class Test : MonoBehaviour
    {
        private EventBus<TestEnum> Bus { get; } = new();

        private void Awake()
        {
            Bus.Subscribe(TestEnum.ONE, () => {Debug.Log("One");});
            Bus.Subscribe(TestEnum.TWO, () => {Debug.Log("Two");});
            Bus.Subscribe(TestEnum.THREE, () => {Debug.Log("Three");});
        }

        private void Start()
        {
            Bus.Publish(TestEnum.ONE);
            Bus.Publish(TestEnum.TWO);
            Bus.Publish(TestEnum.THREE);
        }
    }
}