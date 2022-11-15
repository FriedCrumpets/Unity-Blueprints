using System;

namespace Patterns.StateMachine
{
    [Serializable]
    public class StateException : Exception
    {
        public StateException() {}
        
        public StateException(string message)
            : base(message) { }

        public StateException(string message, Exception inner)
            : base(message, inner) { }
    }
}