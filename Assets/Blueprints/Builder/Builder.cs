#nullable enable
using System;

namespace Blueprints
{
    public interface IBuild<out T>
    {
        T Build();
        T? Destroy();
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

        public T Build() 
            => _execute.Invoke();

        public T? Destroy() 
            => _undo != null ? _undo.Invoke() : default;
    }
    
    public class Builder<T, TT> : IBuild<T>
    {
        private readonly TT _receiver;
        private readonly Func<TT, T> _execute;
        private readonly Func<TT, T>? _undo;
        
        public Builder(TT receiver, Func<TT, T> execute, Func<TT, T>? undo = null)
        {
            _receiver = receiver;
            _execute = execute;
            _undo = undo;
        }
        
        public T Build() 
            => _execute.Invoke(_receiver);

        public T? Destroy() 
            => _undo != null ? _undo.Invoke(_receiver) : default;
    }
}