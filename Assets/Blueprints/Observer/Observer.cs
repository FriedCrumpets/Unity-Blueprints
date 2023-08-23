using System;

namespace Blueprints.Observe
{
    public abstract class Observer<T> : IObserver<T>
    {
        private IDisposable _unSubscriber;

        protected abstract void Completed();
        protected abstract void Error(Exception error);
        protected abstract void Next(T value);

        public void Subscribe(IObservable<T> provider)
        {
            if(provider != null)
                _unSubscriber = provider.Subscribe(this);
        }
        
        void IObserver<T>.OnCompleted()
        {
            Completed(); 
            _unSubscriber.Dispose();
        }

        void IObserver<T>.OnError(Exception error)
            => Error(error);

        void IObserver<T>.OnNext(T value)
            => Next(value);
    }
}