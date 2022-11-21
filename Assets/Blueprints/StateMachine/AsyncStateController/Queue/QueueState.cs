using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blueprints.AsyncStateController.Core;

namespace Blueprints.AsyncStateController.Queue
{
    public abstract class QueueState<TState> : MultiState<TState> where TState : Enum
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
            var tasks = PrepareTasks(StateBehaviours, StateTaskSwitch.Exit);
            await Execute(tasks, OnExitState);
            StateRunning = false;
        }
        
        private static Dictionary<Task, float> PrepareTasks(List<IStateBehaviour> states, StateTaskSwitch taskSwitch)
        {
            return states.ToDictionary(
                state => SelectStateTask(state, taskSwitch), 
                state => SelectStateTime(state, taskSwitch)
            );
        }

        private static async Task Execute(Dictionary<Task, float> tasks, Action action)
        {
            action?.Invoke();

            foreach (var task in tasks)
            {
                task.Key.Start();
                await Task.Delay((int)task.Value * 1000);
            }
        }
    }
}