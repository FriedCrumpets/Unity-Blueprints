using System;
using System.Collections.Generic;
using UnityEngine;

namespace Blueprints.Observer
{
    public class Observable<T> : ScriptableObject, IObservable<T> where T : struct
    {
        [field: SerializeField] public List<IObserver<T>> _observers { get; private set; }= new List<IObserver<T>>();

        protected virtual void Awake() => _observers.ForEach(observer => Subscribe(observer));

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }

            return new UnSubscriber<T>(_observers, observer);
        }

        private class UnSubscriber<TA> : IDisposable
        {
            private List<IObserver<TA>> _observers;
            private IObserver<TA> _observer;

            public UnSubscriber(List<IObserver<TA>> observers, IObserver<TA> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                {
                    _observers.Remove(_observer);
                }
            }
        }

        public virtual void Raise(T? message)
        {
            if (!message.HasValue)
            {
                _observers.ForEach(observer => observer.OnError(new Exception()));
                return;
            }
            
            _observers.ForEach(observer => observer.OnNext(message.Value));
        }

        public virtual void End()
        {
            foreach (var observer in _observers)
            {
                observer.OnCompleted();
            }

            _observers.Clear();
        }
    }
}
