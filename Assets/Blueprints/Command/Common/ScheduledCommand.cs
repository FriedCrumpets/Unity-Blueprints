#nullable enable
using System;
using System.Threading.Tasks;

namespace Blueprints
{
    public interface IScheduleCommand<in T>
    {
        public void Execute(T receiver);
        public void Undo(T receiver);
    }
    
    public class ScheduledCommand<T> : IScheduleCommand<T>
    {
        private readonly Action<T> _execute;
        private readonly Action<T>? _undo;
        
        public ScheduledCommand(Action<T> execute, Action<T>? undo = null)
        {
            _execute = execute;
            _undo = undo;
        }

        public void Execute(T receiver) => 
            _execute.Invoke(receiver);

        public void Undo(T receiver) => 
            _undo?.Invoke(receiver);
    }
    
    public class AsyncScheduledCommand<T> : IScheduleCommand<T>
    {
        private readonly Action<T> _execute;
        private readonly Action<T>? _undo;
        
        public AsyncScheduledCommand(Action<T> execute, Action<T>? undo = null)
        {
            _execute = execute;
            _undo = undo;
        }

        public async void Execute(T receiver) 
            => await Task.Run(() => _execute.Invoke(receiver));

        public async void Undo(T receiver) 
            => await Task.Run(() => _undo?.Invoke(receiver));
    }
}