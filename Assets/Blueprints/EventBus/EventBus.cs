using System;
using System.Collections.Generic;

namespace Blueprints.EventBus
{
    public class EventBus<TEnum> where TEnum : Enum 
    {
        private readonly IDictionary<TEnum, Action> Events = new Dictionary<TEnum, Action>();
        
        public void Subscribe(TEnum eventType, Action observer)
        {
            var eventExists = Events.TryGetValue(eventType, out var thisEvent);

            if (eventExists)
            {
                AddListener(ref thisEvent, observer);
            }
            else
            {
                AddEventToBus(eventType, observer);
            }
        }

        public void UnSubscribe(TEnum eventType, Action observer)
        {
            if (Events.TryGetValue(eventType, out var thisEvent))
            {
                RemoveListener(ref thisEvent, observer);
            }
        }

        public void Publish(TEnum eventType)
        {
            if (Events.TryGetValue(eventType, out var thisEvent))
            {
                thisEvent?.Invoke();
            }
        }

        private void AddListener(ref Action thisEvent, Action observer)
            => thisEvent += observer;
        
        private void RemoveListener(ref Action thisEvent, Action observer)
            => thisEvent -= observer;
        
        private void AddEventToBus(TEnum eventType, Action listener)
        {
            Action thisEvent = delegate { };
            thisEvent += listener;
            Events.Add(eventType, thisEvent);
        }
    }
}