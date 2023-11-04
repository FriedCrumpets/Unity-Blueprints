using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blueprints.AsyncStateController.Core;

namespace Blueprints.AsyncStateController.Async
{
    public abstract class AsyncState<TState> : MultiState<TState> where TState : Enum 
    {
        public override async Task Enter()
        {
            StateRunning = true;
            var tasks = PrepareTasks(StateBehaviours, StateTaskSwitch.Enter);
            await Execute(tasks, OnEnterState);
        }

        public override async Task Idle()
        {
            StateRunning = true;
            var tasks = PrepareTasks(StateBehaviours, StateTaskSwitch.Idle);
            await Execute(tasks, OnIdleState);
        }

        public override async Task Exit()
        {
            var tasks = PrepareTasks(StateBehaviours, StateTaskSwitch.Idle);
            await Execute(tasks, OnExitState);
            StateRunning = false;
        }

        private static IEnumerable<Task> PrepareTasks(List<IStateBehaviour> states, StateTaskSwitch taskSwitch)
        {
            var tasks = new List<Task>();
            var time = 0f;
            
            foreach (var state in states)
            {
                var task = SelectStateTask(state, taskSwitch);
                var timeCheck = SelectStateTime(state, taskSwitch);
                time = timeCheck > time ? timeCheck : time;

                tasks.Add(task);
            }
            
            tasks.Add(Task.Delay((int)time * 1000));

            return tasks;
        }
    }
}