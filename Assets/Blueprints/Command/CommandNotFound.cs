using System;

namespace Blueprints.Command
{
    public class CommandNotFound : Exception
    {
        public CommandNotFound() { }

        public CommandNotFound(string message) : base(message) { }
    }
}