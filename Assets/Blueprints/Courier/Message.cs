using System;

namespace Blueprints.StaticMessaging
{
    /*
     * Future idea for concept
     * 
     * Message is created
     * Sent to Message Silo
     * Courier picks up message each frame
     * Courier Delivers message to Transport
     * Transport dishes out to Buses
     * Buses Publish the Message
     */
    
    
    public class Message
    {
        private readonly Action<object> _message;
        
        private Message(Type recipient, Action<object> message)
        {
            Recipient = recipient;
            _message = message;
        }

        internal Type Recipient { get; }
        
        internal void Receive<T>(T receiver)
            => _message?.Invoke(receiver);
        
        public static Message Create<T>(Action<T> message)
            => new Message(typeof(T), obj => message?.Invoke((T)obj));
    }
    
    // something static that can store internal messages but publicly register 

}