using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.AnimationSys
{
    public class AnimationEvents : MonoBehaviour
    {
        private Dictionary<string, Action> Events { get; set; }

        private void Awake()
            => Events = new Dictionary<string, Action>();

        public IDisposable Observe(string @event, Action callback)
        {
            if( !Events.ContainsKey(@event) )
            {
                Debug.Log($"No event by the name {@event} currently exists");
                Debug.Log($"Creating {@event} callback");
                Events.Add(@event, () => { });
            }

            Events[@event] += callback;

            return new UnSubscriber(this, @event, callback);
        }

        public void Invoke(string @event)
        {
            if(Events.TryGetValue(@event, out var action))
                action?.Invoke();
    #if UNITY_EDITOR
            else
                Debug.LogError($"No event by the name {@event} currently exists");
    #endif
        }

        private void OnDestroy()
            => Events.Clear();

        private class UnSubscriber : IDisposable
        {
            private readonly AnimationEvents _data;
            private readonly string _key;
            private readonly Action _observer;

            public UnSubscriber(AnimationEvents data, string key, Action observer)
            {
                _data = data;
                _key = key;
                _observer = observer;
            }

            public void Dispose()
            {
                if(_data.Events.ContainsKey(_key))
                    _data.Events[_key] -= _observer;
            }
        }
    }
}