using System;
using System.Collections.Generic;

namespace Blueprints.StaticMessaging
{
    public static class Transport
    {
        public static event Action<object, ITypeBussable> NewBus;
        
        private static IDictionary<object, ITypeBussable> Buses;
        
        static Transport()
        {
            Buses = new Dictionary<object, ITypeBussable>();
        }

        public static bool TryAddBus(object owner, ITypeBussable bus)
        {
            var success = Buses.TryAdd(owner, bus);
            
            if(success)
                NewBus?.Invoke(owner, bus);
            
            return success;
        }
        
        public static bool RemoveBus(object owner)
            => Buses.Remove(owner);

        public static ITypeBussable RetrieveBus(object key)
            => Buses.TryGetValue(key, out var value) ? value : default;

        public static void Publish<TKey, TType>(object message)
            => RetrieveBus(typeof(TKey))?.Publish<TType>(message);
    }
}
