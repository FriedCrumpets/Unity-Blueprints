using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Blueprints.StateController
{
    public abstract class State<TState> : MonoBehaviour
    {
        public event Action EnterState;
        public event Action IdleState;
        public event Action ExitState;
        
        [field: SerializeField] public TState CommandingState { get; private set; }
        
        public bool StateRunning { get; protected set; }

        public abstract IEnumerator Enter();
        public abstract IEnumerator Idle();
        public abstract IEnumerator Exit();

        protected void OnEnterState() => EnterState?.Invoke();

        protected void OnIdleState() => IdleState?.Invoke();

        protected void OnExitState() => ExitState?.Invoke();
        
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

        protected static IEnumerator SelectStateTask(IState state, StateTaskSwitch taskSwitch)
        {
            return taskSwitch switch
            {
                StateTaskSwitch.Enter => state.Enter(),
                StateTaskSwitch.Idle => state.Idle(),
                StateTaskSwitch.Exit => state.Exit(),
                _ => throw new ArgumentOutOfRangeException(nameof(taskSwitch), taskSwitch, null)
            };
        }

        protected static float SelectStateTime(IState state, StateTaskSwitch taskSwitch)
        {
            return taskSwitch switch
            {
                StateTaskSwitch.Enter => state.EnterTime,
                StateTaskSwitch.Idle => state.IdleTime,
                StateTaskSwitch.Exit => state.ExitTime,
                _ => throw new ArgumentOutOfRangeException(nameof(taskSwitch), taskSwitch, null)
            };
        }
        
        protected IEnumerator Execute(IEnumerable<IEnumerator> tasks, Action onStart = null, Action onComplete = null)
        {
            onStart?.Invoke();
            
            var enumerable = tasks as IEnumerator[] ?? tasks.ToArray();
            
            foreach (var task in enumerable)
            {
                yield return StartCoroutine(task);
            }
            
            onComplete?.Invoke();
        }

        protected IEnumerator WaitForSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }
    }
}