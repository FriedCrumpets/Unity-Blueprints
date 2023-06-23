using System;
using UnityEngine;

namespace Blueprints
{
    public class CountAction
    {
        public string LOGString = "Completed";
        
        public CountAction(int count, Action action = null)
        {
            if (action == null)
                Action = () => { };
            
            if(count == 0)
            {
                Action?.Invoke();
                return;
            }

            Count = count;
            Action = action;
        }
        
        public int Count { get; private set; }
        public Action Action { get; }

        public void Decrement()
        {
            if (--Count > 0) 
                return;
            
            Action?.Invoke();
            Debug.Log($"{nameof(CountAction)}: {LOGString}");
        }
    }
}