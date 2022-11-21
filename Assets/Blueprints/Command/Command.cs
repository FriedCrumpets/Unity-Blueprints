#nullable enable
using System;

namespace Blueprints.Command
{
    public class Command<T> : ICommand, IBufferrable
    {
        public Command(T receiver, Action<T> execute, Action<T>? undo = null)
        {
            _receiver = receiver;
            _execute = execute;
            _undo = undo;
        }

        private readonly T _receiver;
        private readonly Action<T> _execute;
        private readonly Action<T>? _undo;

        /// <summary>
        /// Immediately Execute the command into the Command Stream
        /// </summary>
        public void Execute() => _execute.Invoke(_receiver);

        /// <summary>
        /// Immediately undo the command and set its state into the command stream
        /// </summary>
        public void Undo() => _undo?.Invoke(_receiver);

        public void Buffer() => CommandBuffer.Buffer(this);
    }
}