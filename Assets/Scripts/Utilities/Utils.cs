using UnityEngine;

namespace Blueprints.Facade
{
    public static class Utils
    {
        public static string XRSettingsPath = $"{Application.dataPath}/Features/XR/Resources";

        private static Camera _mainCamera;
        
        public static Camera MainCamera => _mainCamera ??= Camera.main;

        public static T ForceComponent<T>(this GameObject obj) where T : Component
        {
            var component = obj.GetComponent<T>();
            
            return component == null ? obj.AddComponent<T>() : component;
        }

        public static T ForceScriptableObject<T>(this T obj, string path) where T : ScriptableObject
        {
            if (obj == null)
            {
                obj = Resources.Load<T>(path);
                if (obj == null)
                {
                    obj = ScriptableObject.CreateInstance<T>();
                }
            }

            return obj;
        }
        
        public static GameObject CreateGameObject(Transform parent, string name)
        {
            return new GameObject(name)
            {
                transform = { parent = parent }
            };
        }
    }
}