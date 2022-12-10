using System;
using System.Threading.Tasks;

namespace Blueprints.StateMachine.AsyncStates.Core
{
    public class StateAsync : IStateAsync
    {
        public StateAsync() { }

        public StateAsync(Action enter, Action exit)
        {
            EnterAction = enter;
            ExitAction = exit;
        }

        public StateAsync(Action enter, Action idle, Action exit)
        {
            EnterAction = enter;
            IdleAction = idle;
            ExitAction = exit;
        }
        
        private bool _idle = true;
        private uint _idleTime = 500;
        private uint _maxIdleRepeats = 100;
     
        public Action EnterAction { get; }
        public Action IdleAction { get; }
        public Action ExitAction { get; }

        public uint IdleTime
        {
            get => _idleTime;
            set
            {
                if (value < 100)
                {
                    throw new ArgumentException($"{this} state: Idle Time has a minimum value of 100");
                }

                _idleTime = value;
            }
        }

        public uint MaximumIdleRepeats
        {
            get => _maxIdleRepeats;
            set
            {
                if (_maxIdleRepeats > 100)
                {
                    throw new ArgumentException($"{this} state: Maximum Idle Repeats cannot be set above 100");
                }

                _maxIdleRepeats = value;
            }
        }

        public void Continue() => _idle = false;

        public virtual Task Enter(Action action = null)
        {
            return action != null ? Task.Run(action) : Task.CompletedTask;
        }

        public virtual Task Idle(Action action = null)
        {

            if (IdleAction != null)
            {
                var repeats = 0;
                
                do
                {
                    action?.Invoke();
                    Task.Delay((int)IdleTime);
                    repeats++;
                } while (_idle || repeats > MaximumIdleRepeats);   
            }

            return Task.CompletedTask;
        }

        public virtual Task Exit(Action action = null)
        {
            return action != null ? Task.Run(action) : Task.CompletedTask;
        }

        public async void Execute()
        {
            await Enter(EnterAction);
            await Idle(IdleAction);
            await Exit(ExitAction);
        }
    }
}