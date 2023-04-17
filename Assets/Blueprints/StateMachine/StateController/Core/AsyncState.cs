using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Blueprints.StateController
{
    public abstract class AsyncState<TState> : MultiState<TState> where TState : Enum 
    {
        public override IEnumerator Enter()
        {
            StateRunning = true;
            var tasks = PrepareTasks(Behaviours, StateTaskSwitch.Enter);
            yield return StartCoroutine(Execute(tasks, OnEnterState));
        }

        public override IEnumerator Idle()
        {
            StateRunning = true;
            var tasks = PrepareTasks(Behaviours, StateTaskSwitch.Idle);
            yield return StartCoroutine(Execute(tasks, OnIdleState));
        }

        public override IEnumerator Exit()
        {
            var tasks = PrepareTasks(Behaviours, StateTaskSwitch.Exit);
            yield return StartCoroutine(Execute(tasks, OnExitState));
            StateRunning = false;
        }
        
        private static IEnumerable<IEnumerator> PrepareTasks(IEnumerable<IState> states, StateTaskSwitch taskSwitch)
        {
            return states.Select(
                state => SelectStateTask(state, taskSwitch)
            );
        }

        protected new IEnumerator Execute(IEnumerable<IEnumerator> tasks, Action onStarted = null,
            Action onCompleted = null)
        {
            onStarted?.Invoke();

            foreach (var task in tasks)
            {
                yield return StartCoroutine(task);
            }
            
            onCompleted?.Invoke();
        }
    }
}