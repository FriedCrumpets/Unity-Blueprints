#nullable enable
using System;

namespace Blueprints
{
    public interface IBuild<out T>
    {
        T Execute();
        T? Undo();
    }
    
    public class Builder<T> : IBuild<T>
    {
        private readonly Func<T> _execute;
        private readonly Func<T>? _undo;
        
        public Builder(Func<T> execute, Func<T>? undo = null)
        {
            _execute = execute;
            _undo = undo;
        }

        /// <summary>
        /// Immediately Execute the command into the Command Stream
        /// </summary>
        public T Execute() => _execute.Invoke();

        /// <summary>
        /// Immediately undo the command and set its state into the command stream
        /// </summary>
        public T? Undo() => _undo != null ? _undo.Invoke() : default;
    }
    
    public class Builder<T1, T2> : IBuild<T1>
    {
        private readonly T2 _receiver;
        private readonly Func<T2, T1> _execute;
        private readonly Func<T2, T1>? _undo;
        
        public Builder(T2 receiver, Func<T2, T1> execute, Func<T2, T1>? undo = null)
        {
            _receiver = receiver;
            _execute = execute;
            _undo = undo;
        }
        
        /// <summary>
        /// Immediately Execute the command into the Command Stream
        /// </summary>
        public T1 Execute() => _execute.Invoke(_receiver);

        /// <summary>
        /// Immediately undo the command and set its state into the command stream
        /// </summary>
        public T1? Undo() => _undo != null ? _undo.Invoke(_receiver) : default;
    }
}