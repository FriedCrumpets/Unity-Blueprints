using System;
using System.Collections.Generic;

namespace Blueprints.StaticMessaging
{
    public class TypeBus : ITypeBussable
    {
        private IDictionary<object, Action<object>> Bus { get; } 
            = new Dictionary<object, Action<object>>();

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
            private readonly TypeBus _bus;
            private readonly Action<object> _observer;

            public UnSubscriber(TypeBus bus, Action<object> observer)
            {
                _bus = bus;
                _observer = observer;
            }

            public void Dispose()
                => _bus.Bus[typeof(T)] -= _observer;
        }
    }
}