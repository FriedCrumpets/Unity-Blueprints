using System;
using UnityEngine;

namespace Blueprints.Facade
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "ExampleSettingsAsset", menuName = "ScriptableObjects/Settings/Example", order = 0)]
    public class ExampleSettings : ScriptableObject
    {
        [SerializeField] public Setting<float> exampleSetting1;
        [SerializeField] public Setting<string> exampleSetting2;
        [SerializeField] public Setting<int> exampleSetting3;
        [SerializeField] public Setting<DateTime> exampleSetting4;
    }
}