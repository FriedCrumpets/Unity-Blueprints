using System;
using System.Collections.Generic;

namespace Blueprints.EventBus.Object
{
    public class EventBus : IBus
    {
        private IDictionary<object, Action<object>> Bus { get; }

        public EventBus(object type)
        {
            Type = type;
            Bus = new Dictionary<object, Action<object>>();
        }

        public object Type { get; }
        
        public IDisposable Subscribe(object obj, Action<object> observer)
        {
            if( Bus.ContainsKey(obj) )
                Bus[obj] += observer;
            else
                Bus.Add(obj, observer);

            return new UnSubscriber(this, obj, observer);
        }

        public void Publish( object obj, object message)
        {
            if(Bus.TryGetValue(obj, out var action))
                action?.Invoke(message);
        }

        private class UnSubscriber : IDisposable
        {
            private readonly EventBus _bus;
            private readonly object _obj;
            private readonly Action<object> _observer;

            public UnSubscriber(EventBus bus, object obj, Action<object> observer)
            {
                _bus = bus;
                _obj = obj;
                _observer = observer;
            }

            public void Dispose()
                => _bus.Bus[_obj] -= _observer;
        }
    }

}