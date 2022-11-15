using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RequireInterface;
using UnityEngine;

namespace Patterns.StateMachine.Core
{
    internal enum StateTaskSwitch
    {
        None,
        Enter,
        Idle,
        Exit,
    }
    
    public abstract class AsyncState<TState> : State<TState> where TState : Enum 
    {
        public event Action EnterState;
        public event Action IdleState;
        public event Action ExitState;

        private List<IStateBehaviourAsync> _behaviours;

        [field: SerializeField] public TState State { get; private set; }
        
        [field: SerializeField, RequireInterface(typeof(IStateBehaviourAsync))]
        public List<UnityEngine.Object> behaviours { get; private set; }
        
        public List<IStateBehaviourAsync> Behaviours => 
            _behaviours ??= behaviours.Select(item => item as IStateBehaviourAsync).ToList(); 
        
        public bool StateRunning { get; private set; }

        protected virtual void OnEnable()
        {
            if (behaviours == null)
            {
                throw new StateException($"State '{name}':  IState Behaviour unassigned");
            }
        }

        protected virtual void OnDisable()
        {
            if (behaviours == null)
            {
                throw new StateException($"State '{name}':  IState Behaviour unassigned");
            }
        }

        public override async Task Enter()
        {
            StateRunning = true;
            var enterTasks = CreateTaskListing(Behaviours, StateTaskSwitch.Enter);
            await Execute(EnterState, enterTasks);
        }

        public override async Task Idle()
        {
            StateRunning = true;
            var idleTasks = CreateTaskListing(Behaviours, StateTaskSwitch.Idle);
            await Execute(IdleState, idleTasks);
        }

        public override async Task Exit()
        {
            var exitTasks = CreateTaskListing(Behaviours, StateTaskSwitch.Idle);
            await Execute(ExitState, exitTasks);
            StateRunning = false;
        }

        private static IEnumerable<Task> CreateTaskListing(List<IStateBehaviourAsync> states, StateTaskSwitch taskSwitch)
        {
            var taskList = new List<Task>();
            float time = 0f;
            
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

        private static Task SelectStateTask(IStateBehaviourAsync state, StateTaskSwitch taskSwitch)
        {
            return taskSwitch switch
            {
                StateTaskSwitch.Enter => state.Enter(),
                StateTaskSwitch.Idle => state.Idle(),
                StateTaskSwitch.Exit => state.Exit(),
                _ => throw new ArgumentOutOfRangeException(nameof(taskSwitch), taskSwitch, null)
            };
        }

        private static float SelectStateTime(IStateBehaviourAsync state, StateTaskSwitch taskSwitch)
        {
            return taskSwitch switch
            {
                StateTaskSwitch.Enter => state.EnterTime,
                StateTaskSwitch.Idle => state.IdleTime,
                StateTaskSwitch.Exit => state.ExitTime,
                _ => throw new ArgumentOutOfRangeException(nameof(taskSwitch), taskSwitch, null)
            };
        }

        private static async Task Execute(Action stateAction, IEnumerable<Task> tasks)
        {
            stateAction?.Invoke();

            var enumerable = tasks as Task[] ?? tasks.ToArray();
            
            foreach (var task in enumerable)
            {
                task.Start();
            }
            
            await Task.WhenAll(enumerable);
        }
    }
}