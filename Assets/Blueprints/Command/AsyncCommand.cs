#nullable enable
using System;
using System.Threading.Tasks;

namespace Blueprints
{
    public class AsyncCommand<T> : ICommand
    {
        public AsyncCommand(T receiver, Action<T> execute, Action<T>? undo = null)
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
        public void Execute() => Task.Run(ExecuteAsync);

        private Task ExecuteAsync()
        {
            _execute.Invoke(_receiver);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Immediately undo the command and set its state into the command stream
        /// </summary>
        public void Undo() => Task.Run(UndoAsync);

        private Task UndoAsync()
        {
            _undo?.Invoke(_receiver);
            return Task.CompletedTask;
        }

        public void Buffer() => CommandBuffer.Buffer(this);
    }
}