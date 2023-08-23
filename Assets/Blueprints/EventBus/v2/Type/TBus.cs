using System;
using System.Collections.Generic;

namespace Blueprints.EventBus
{
    public class TBus : ITBus
    {
        private IDictionary<Type, Action<object>> Bus { get; }

        public TBus()
        {
            Bus = new Dictionary<Type, Action<object>>();
        }

        public IDisposable Subscribe<T>(Action<object> observer)
        {
            if( Bus.ContainsKey(typeof(T)) )
                Bus[typeof(T)] += observer;
            else
                Bus.Add(typeof(T), observer);

            return new UnSubscriber<T>(this, observer);
        }
        
        public void Publish<T>(object message)
        {
            if(Bus.TryGetValue(typeof(T), out var action))
                action?.Invoke(message);
        }
        
        private class UnSubscriber<T> : IDisposable
        {
            private readonly TBus _bus;
            private readonly Action<object> _observer;

            public UnSubscriber(TBus bus, Action<object> observer)
            {
                _bus = bus;
                _observer = observer;
            }

            public void Dispose()
                => _bus.Bus[typeof(T)] -= _observer;
        }
    }
}