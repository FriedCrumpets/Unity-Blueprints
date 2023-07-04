using System;
using Blueprints.DoD;
using UnityEngine;

namespace Blueprints.Facade
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "ExampleSettingsAsset", menuName = "ScriptableObjects/Settings/Example", order = 0)]
    public class ExampleSettings : ScriptableObject
    {
        [SerializeField] public Data<float> exampleSetting1;
        [SerializeField] public Data<string> exampleSetting2;
        [SerializeField] public Data<int> exampleSetting3;
        [SerializeField] public Data<DateTime> exampleSetting4;
    }
}