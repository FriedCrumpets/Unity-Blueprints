#nullable enable
using System;
using System.Threading.Tasks;

namespace Blueprints
{
    public interface ICommand
    {
        public void Execute();
        public void Undo();
    }

    public class Command : ICommand
    {
        private readonly Action _execute;
        private readonly Action? _undo;
        
        public Command(Action execute, Action? undo = null)
        {
            _execute = execute;
            _undo = undo;
        }

        /// <summary>
        /// Immediately Execute the command into the Command Stream
        /// </summary>
        public void Execute()
            => _execute.Invoke();

        /// <summary>
        /// Immediately undo the command and set its state into the command stream
        /// </summary>
        public void Undo() 
            => _undo?.Invoke();
        
        public void Buffer() 
            => CommandBuffer.Buffer(this);
    }
    
    public class Command<T> : ICommand
    {
        private readonly T _receiver;
        private readonly Action<T> _execute;
        private readonly Action<T>? _undo;
        
        public Command(T receiver, Action<T> execute, Action<T>? undo = null)
        {
            _receiver = receiver;
            _execute = execute;
            _undo = undo;
        }

        /// <summary>
        /// Immediately Execute the command into the Command Stream
        /// </summary>
        public void Execute() => 
            _execute.Invoke(_receiver);

        /// <summary>
        /// Immediately undo the command and set its state into the command stream
        /// </summary>
        public void Undo() => 
            _undo?.Invoke(_receiver);

        public void Buffer() 
            => CommandBuffer.Buffer(this);
    }
    
    public class AsyncCommand<T> : ICommand
    {
        private readonly T _receiver;
        private readonly Action<T> _execute;
        private readonly Action<T>? _undo;
        
        public AsyncCommand(T receiver, Action<T> execute, Action<T>? undo = null)
        {
            _receiver = receiver;
            _execute = execute;
            _undo = undo;
        }
        
        public async void Execute() 
            => await Task.Run(() => _execute?.Invoke(_receiver));

        public async void Undo() 
            => await Task.Run(() => _undo?.Invoke(_receiver));

        public void Buffer() 
            => CommandBuffer.Buffer(this);
    }
}