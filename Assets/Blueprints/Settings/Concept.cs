using UnityEngine;

namespace Blueprints.Settings
{
    [System.Serializable]
    public class Concept
    {
        public Concept_SO storage;

        public float Setting1
        {
            get => storage.Setting1;
            set
            {
                // do things
                storage.Setting1 = value;
            }
        }

        public string Setting2
        {
            get => storage.Setting2;
            set => storage.Setting2 = value;
        }

        public bool Setting3
        {
            get => storage.Setting3;
            set => storage.Setting3 = value;
        }
    }
}