using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

namespace Editor
{
    public static class EditorUtils
    {
        public static VisualElement CreateAsset(string assetPath)
        {
            var uiAsset =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(assetPath);
            
            return uiAsset.Instantiate();
        }
        
        public static string StringToElementName(string name) => 
            string.Join("", name.Split(" "));
        
        public static string StringToElementClass(string className) => 
            string.Join("-", className.ToLower().Split(" "));

        public static VisualElement FindElementByName(IEnumerable<VisualElement> elements, string name)
        {
            return elements.FirstOrDefault(
                element => element.name == EditorUtils.StringToElementName(name)
            );
        }
    }

    public class EditorException : Exception
    {
        public EditorException(string message) : base(message) { }
    }
}