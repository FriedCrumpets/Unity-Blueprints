using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils
{
    public static partial class Utilities
    {
        public static T ForceComponent<T>(this GameObject gameObject) where T : Component
        {
            var gComponent = gameObject.GetComponent<T>();
            return gComponent == null ? gameObject.AddComponent<T>() : gComponent;
        }

        public static T LoadResourceIfNull<T>(T resource, string path) where T : Object
        {
            if (resource != null)
                return resource;
            
            resource = Resources.Load<T>(path);
            if (resource == null)
                throw new TypeLoadException($"{typeof(T)} was not found at location: {path}");

            return resource;
        }
        
        public static bool NullCheck(object check) => check == null;
    }   
}
