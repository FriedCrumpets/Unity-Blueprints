using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Blueprints.StateMachine.Core
{
    public abstract class State<TState> : MonoBehaviour
    {
        public event Action EnterState;
        public event Action IdleState;
        public event Action ExitState;
        
        [field: SerializeField] public TState CommandingState { get; private set; }
        
        public abstract Task Enter();
        public abstract Task Idle();
        public abstract Task Exit();

        protected void OnEnterState()
        {
            EnterState?.Invoke();
        }

        protected void OnIdleState()
        {
            IdleState?.Invoke();
        }

        protected void OnExitState()
        {
            ExitState?.Invoke();
        }
        
        protected Action SelectStateAction(StateTaskSwitch taskSwitch)
        {
            return taskSwitch switch
            {
                StateTaskSwitch.Enter => EnterState,
                StateTaskSwitch.Idle => IdleState,
                StateTaskSwitch.Exit => ExitState,
                _ => throw new ArgumentOutOfRangeException(nameof(taskSwitch), taskSwitch, null)
            };
        }

        protected static Task SelectStateTask(IStateBehaviour state, StateTaskSwitch taskSwitch)
        {
            return taskSwitch switch
            {
                StateTaskSwitch.Enter => state.Enter(),
                StateTaskSwitch.Idle => state.Idle(),
                StateTaskSwitch.Exit => state.Exit(),
                _ => throw new ArgumentOutOfRangeException(nameof(taskSwitch), taskSwitch, null)
            };
        }

        protected static float SelectStateTime(IStateBehaviour state, StateTaskSwitch taskSwitch)
        {
            return taskSwitch switch
            {
                StateTaskSwitch.Enter => state.EnterTime,
                StateTaskSwitch.Idle => state.IdleTime,
                StateTaskSwitch.Exit => state.ExitTime,
                _ => throw new ArgumentOutOfRangeException(nameof(taskSwitch), taskSwitch, null)
            };
        }
        
        protected static async Task Execute(IEnumerable<Task> tasks, Action action)
        {
            action?.Invoke();
            
            var enumerable = tasks as Task[] ?? tasks.ToArray();
            
            foreach (var task in enumerable)
            {
                task.Start();
            }
            
            await Task.WhenAll(enumerable);
        }
    }
}