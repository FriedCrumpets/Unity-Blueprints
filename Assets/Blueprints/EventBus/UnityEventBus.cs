using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Blueprints.UnityBus
{
    public class EventBus<TEnum> where TEnum : Enum
    {
        private readonly IDictionary<TEnum, UnityEvent> _events = new Dictionary<TEnum, UnityEvent>();

        public void Subscribe(TEnum eventType, UnityAction listener)
        {
            var eventExists = _events.TryGetValue(eventType, out var thisEvent);

            if (eventExists)
            {
                AddListener(thisEvent, listener);
            }
            else
            {
                AddEventToBus(eventType, listener);
            }
        }

        public void UnSubscribe(TEnum eventType, UnityAction listener)
        {
            if (_events.TryGetValue(eventType, out var thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public void Publish(TEnum eventType)
        {
            if (_events.TryGetValue(eventType, out var thisEvent))
            {
                thisEvent?.Invoke();
            }
        }

        private static void AddListener(UnityEvent thisEvent, UnityAction listener)
        {
            thisEvent.AddListener(listener);
        }

        private void AddEventToBus(TEnum eventType, UnityAction listener)
        {
            var thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            _events.Add(eventType, thisEvent);
        }
    }
    
    public abstract class EventBus<TEnum, TInterface> where TEnum : Enum 
    {
        private readonly IDictionary<TEnum, Action> Events = new Dictionary<TEnum, Action>();
        
        public void Subscribe(TEnum eventType, TInterface observer)
        {
            var eventExists = Events.TryGetValue(eventType, out var thisEvent);

            if (eventExists)
            {
                AddListener(thisEvent, observer);
            }
            else
            {
                AddEventToBus(eventType, observer);
            }
        }

        public void UnSubscribe(TEnum eventType, TInterface observer)
        {
            if (Events.TryGetValue(eventType, out var thisEvent))
            {
                RemoveListener(thisEvent, observer);
            }
        }

        public void Publish(TEnum eventType)
        {
            if (Events.TryGetValue(eventType, out var thisEvent))
            {
                thisEvent?.Invoke();
            }
        }

        protected abstract void AddEventToBus(TEnum eventType, TInterface listener);
        protected abstract void AddListener(Action thisEvent, TInterface observer);
        protected abstract void RemoveListener(Action thisEvent, TInterface observer);
    }
}