using System;
using System.Collections.Generic;

namespace Blueprints.EventBus
{
    public static class Transport
    {
        public static event Action<object, ITBus> NewTransport;
        
        private static IDictionary<object, ITBus> Buses;
        
        static Transport()
        {
            Buses = new Dictionary<object, ITBus>();
        }

        public static bool AddBus(object owner, ITBus bus)
        {
            var success = Buses.TryAdd(owner, bus);
            
            if(success)
                NewTransport?.Invoke(owner, bus);
            
            return success;
        }

        public static bool RemoveBus(object owner)
            => Buses.Remove(owner);

        public static ITBus RetrieveBus(object key)
            => Buses.TryGetValue(key, out var value) ? value : default;
    }
}
