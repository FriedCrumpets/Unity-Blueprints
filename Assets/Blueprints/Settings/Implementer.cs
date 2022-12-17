using UnityEngine;

namespace Blueprints.Settings
{
    public class Implementer : MonoBehaviour
    {
        [SerializeField] private Concept settings;
    
        private void Start()
        {
            settings.cachedStorage.LoadSettings();
        }

        private void OnDestroy()
        {
            settings.cachedStorage.SaveSettings();
        }
    }
}
