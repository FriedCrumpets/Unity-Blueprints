using System;
using System.Collections.Generic;
using UnityEngine;

namespace Blueprints.Messenger
{
    public class Courier
    {
        private IDictionary<Type, object> Addresses { get; } = new Dictionary<Type, object>();

        public void Send(Message message)
        {
            if( Addresses.TryGetValue(message.Recipient, out var obj) )
                message.Receive(obj);
#if UNITY_EDITOR
            else
                Debug.LogWarning($"Failed To send Message to {message.Recipient}; Recipient not registered with {typeof(Courier)}");
#endif
        }

        public IDisposable Register(object obj)
            => Addresses.TryAdd(obj.GetType(), obj) ? new Subscription(this, obj) : null;
        
        private class Subscription : IDisposable
        {
            private readonly object _obj;
            private readonly Courier _courier;

            public Subscription(Courier courier, object obj)
            {
                _courier = courier;
                _obj = obj;
            }
            
            public void Dispose()
                => _courier.Addresses.Remove(_obj.GetType());
        }
    }
}