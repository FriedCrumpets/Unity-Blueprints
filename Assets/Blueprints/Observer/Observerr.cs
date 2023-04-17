using UnityEngine;

namespace Observer
{
    public class Observerr : IObserve
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