using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blueprints.StateMachine.Core;

namespace Blueprints.StateMachine.Async
{
    public abstract class AsyncState<TState> : MultiState<TState> where TState : Enum 
    {
        public override async Task Enter()
        {
            StateRunning = true;
            var enterTasks = PrepareTasks(StateBehaviours, StateTaskSwitch.Enter);
            await Execute(enterTasks, OnEnterState);
        }

        public override async Task Idle()
        {
            StateRunning = true;
            var idleTasks = PrepareTasks(StateBehaviours, StateTaskSwitch.Idle);
            await Execute(idleTasks, OnIdleState);
        }

        public override async Task Exit()
        {
            var exitTasks = PrepareTasks(StateBehaviours, StateTaskSwitch.Idle);
            await Execute(exitTasks, OnExitState);
            StateRunning = false;
        }

        private static IEnumerable<Task> PrepareTasks(List<IStateBehaviour> states, StateTaskSwitch taskSwitch)
        {
            var taskList = new List<Task>();
            var time = 0f;
            
            foreach (var state in states)
            {
                var task = SelectStateTask(state, taskSwitch);
                var timeCheck = SelectStateTime(state, taskSwitch);
                time = timeCheck > time ? timeCheck : time;

                taskList.Add(task);
            }
            
            taskList.Add(Task.Delay((int)time * 1000));

            return taskList;
        }
    }
}