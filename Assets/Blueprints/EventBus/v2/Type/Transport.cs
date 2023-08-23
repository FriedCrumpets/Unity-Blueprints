using System.Collections.Generic;

namespace Blueprints.EventBus
{
    public static class Transport
    {
        private static IDictionary<object, ITBus> Buses; 
        
        static Transport()
        {
            Buses = new Dictionary<object, ITBus>();
        }

        public static bool AddBus(object owner, ITBus bus)
            => Buses.TryAdd(owner, bus);

        public static bool RemoveBus(object owner)
            => Buses.Remove(owner);

        public static ITBus RetrieveBus(object key)
            => Buses.TryGetValue(key, out var value) ? value : default;
    }
}
