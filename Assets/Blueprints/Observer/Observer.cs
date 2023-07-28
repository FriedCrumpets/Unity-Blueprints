using UnityEngine;

namespace Blueprints.Observe
{
    public class Observer : IObserve
    {
        public void Observe(IObservable observable)
        {
            observable.Subscribe(this);
        }

        public void Execute()
        {
            Debug.Log("hooch is crazy");
        }
    }
}