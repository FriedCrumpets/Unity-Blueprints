using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blueprints.StateController.Core;
using UnityEngine;

namespace Blueprints.StateController.Queue
{
    public abstract class QueueState<TState> : MultiState<TState> where TState : Enum
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

        private static Dictionary<IEnumerator, float> PrepareTasks(List<IState> states, StateTaskSwitch taskSwitch)
        {
            return states.ToDictionary(
                state => SelectStateTask(state, taskSwitch),
                state => SelectStateTime(state, taskSwitch)
            );
        }

        protected new IEnumerator Execute(Dictionary<IEnumerator, float> tasks, Action onStarted = null,
            Action onCompleted = null)
        {
            onStarted?.Invoke();

            foreach (var task in tasks)
            {
                yield return StartCoroutine(task.Key);
                yield return new WaitForSeconds(task.Value);
            }
        }
    }
}