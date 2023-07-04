using Blueprints.DoD;
using Logging;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Blueprints.Facade.Editor
{
    [CustomPropertyDrawer(typeof(Data<float>))]
    public class FloatSettingsDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return new PropertyField(property.FindPropertyRelative("value"))
            {
                label = property.displayName
            };
        }
    }
    
    [CustomPropertyDrawer(typeof(Data<int>))]
    public class IntSettingsDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return new PropertyField(property.FindPropertyRelative("value"))
            {
                label = property.displayName
            };
        }
    }
    
    [CustomPropertyDrawer(typeof(Data<string>))]
    public class StringSettingsDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return new PropertyField(property.FindPropertyRelative("value"))
            {
                label = property.displayName
            };
        }
    }
    
    [CustomPropertyDrawer(typeof(Data<bool>))]
    public class BoolSettingsDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return new PropertyField(property.FindPropertyRelative("value"))
            {
                label = property.displayName
            };
        }
    }
}