using System;
using UnityEngine;

namespace Blueprints.Facade
{
    public class TestSettings : MonoBehaviour
    {
        [SerializeField] private ExampleSettings exampleSettings;

        private void Awake()
        {
            exampleSettings.exampleSetting1.Value = 1f;
            exampleSettings.exampleSetting2.Value = "poo";
            exampleSettings.exampleSetting3.Value = 3;
            exampleSettings.exampleSetting4.Value = DateTime.UtcNow;
        }

        [ContextMenu("Save")]
        public void Save()
        {
            exampleSettings.Save();
        }

        [ContextMenu("Load")]
        public void Load()
        {
            exampleSettings.Load();
        }
    }
}