using System;
using System.Collections.Generic;
using Blueprints.Messenger;
using UnityEngine;

namespace Blueprints.StaticMessenger
{
    public static class Courier
    {
        private static Dictionary<Type, object> Addresses { get; }

        static Courier()
        {
            Addresses = new Dictionary<Type, object>();
        }

        public static void Send(this Message message)
        {
            if( Addresses.TryGetValue(message.Recipient, out var obj) )
                message.Receive(obj);
            else
                Debug.LogWarning($"Failed To send Message to {message.Recipient}; Recipient not registered with {typeof(Courier)}");
        }

        public static IDisposable Register(object obj)
            => Addresses.TryAdd(obj.GetType(), obj) ? new DeRegister(obj) : null;
        
        private class DeRegister : IDisposable
        {
            private readonly object _obj;
            
            public DeRegister(object obj) { _obj = obj; }
            
            public void Dispose()
                => Addresses.Remove(_obj.GetType());
        }
    }
}