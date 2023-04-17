using System;

namespace Blueprints
{
    public class CommandNotFound : Exception
    {
        public CommandNotFound() { }

        public CommandNotFound(string message) : base(message) { }
    }
}