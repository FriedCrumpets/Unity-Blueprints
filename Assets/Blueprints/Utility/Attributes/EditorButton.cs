using System;
using UnityEngine;

namespace Attributes
{
    [AttributeUsage( AttributeTargets.Method )]
    public class EditorButton : PropertyAttribute
    {
        public EditorButton(string name, Action callback)
        {
            Name = name;
            Callback = callback;
        }
        
        public string Name { get; }
        public Action Callback { get; }

        public void Invoke()
            => Callback?.Invoke();
    }
}