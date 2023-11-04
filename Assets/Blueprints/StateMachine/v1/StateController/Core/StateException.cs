using System;

namespace Blueprints.StateController
{
    public class StateException : Exception
    {
        public StateException(string message) : base(message) { }
    }
}