using System;
using System.Collections.Generic;

namespace Blueprints.StaticMessaging
{
    public static class GlobalBus
    {
        private static IDictionary<System.Type, Action<object>> Bus { get; }

        static GlobalBus()
        {
            Bus = new Dictionary<System.Type, Action<object>>();
        }

        public static IDisposable Subscribe<T>(Action<object> observer)
        {
            if( Bus.ContainsKey(typeof(T)) )
                Bus[typeof(T)] += observer;
            else
                Bus.Add(typeof(T), observer);

            return new UnSubscriber<T>(observer);
        }

        public static void Publish<T>(object message, bool typeStrict = true)
        {
            if( typeStrict && (T)message == null )
                return;
            
            if(Bus.TryGetValue(typeof(T), out var action))
                action?.Invoke(message);
        }

        private class UnSubscriber<T> : IDisposable
        {
            private readonly Action<object> _observer;

            public UnSubscriber(Action<object> observer)
            {
                _observer = observer;
            }

            public void Dispose()
                => Bus[typeof(T)] -= _observer;
        }
    }
}