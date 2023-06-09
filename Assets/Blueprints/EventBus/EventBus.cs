using System;
using System.Collections.Generic;

namespace Blueprints.EventBus
{
    public class EventBus<TEnum> where TEnum : Enum
    {
        private IDictionary<TEnum, Action> Events { get; }

        public EventBus()
        {
            Events = new Dictionary<TEnum, Action>();
        }

        public void Subscribe(TEnum eventType, Action observer)
        {
            if (!Events.TryGetValue(eventType, out var thisEvent))
                thisEvent = () => { };
        
            thisEvent += observer;
            Events[eventType] = thisEvent;
        }

        public void UnSubscribe(TEnum eventType, Action observer)
        {
            if (Events.TryGetValue(eventType, out var thisEvent))
            {
                thisEvent -= observer;
                Events[eventType] = thisEvent;
            }
        }

        public void Publish(TEnum eventType)
        {
            if (Events.TryGetValue(eventType, out var thisEvent))
                thisEvent?.Invoke();
        }
    }

    public class EventBus
    {
        private IDictionary<Type, Action> Events { get; }

        public EventBus()
        {
            Events = new Dictionary<Type, Action>();
        }

        public void Subscribe<T>(Action observer)
        {
            if (!Events.TryGetValue(typeof(T), out var thisEvent))
                thisEvent = () => { };
        
            thisEvent += observer;
            Events[typeof(T)] = thisEvent;
        }

        public void UnSubscribe<T>(Action observer)
        {
            if (!Events.TryGetValue(typeof(T), out var thisEvent)) 
                return;
            
            thisEvent -= observer;
            Events[typeof(T)] = thisEvent;
        }

        public void Publish<T>()
        {
            if (Events.TryGetValue(typeof(T), out var thisEvent))
                thisEvent?.Invoke();
        }
    }
}