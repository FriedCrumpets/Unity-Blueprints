#nullable enable
using System;

namespace Blueprints
{
    public class Command : ICommand
    {
        public Command(Action execute, Action? undo = null)
        {
            _execute = execute;
            _undo = undo;
        }
        
        private readonly Action _execute;
        private readonly Action? _undo;

        /// <summary>
        /// Immediately Execute the command into the Command Stream
        /// </summary>
        public void Execute() => _execute.Invoke();

        /// <summary>
        /// Immediately undo the command and set its state into the command stream
        /// </summary>
        public void Undo() => _undo?.Invoke();
    }
    
    public class Command<T> : ICommand
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

    public class Modifier<T>
    {
        public Modifier(Func<T> execute, Func<T>? undo = null)
        {
            _execute = execute;
            _undo = undo;
        }
        
        private readonly Func<T> _execute;
        private readonly Func<T>? _undo;

        /// <summary>
        /// Immediately Execute the command into the Command Stream
        /// </summary>
        public T Execute() => _execute.Invoke();

        /// <summary>
        /// Immediately undo the command and set its state into the command stream
        /// </summary>
        public T? Undo() => _undo != null ? _undo.Invoke() : default;
    }
}