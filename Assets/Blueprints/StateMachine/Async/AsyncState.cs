using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blueprints.StateMachine.Core;
using RequireInterface;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Blueprints.StateMachine.Async
{
    public abstract class AsyncState<TState> : State<TState> where TState : Enum 
    {
        private List<IStateBehaviour> _stateBehaviours;

        [field: SerializeField] public TState State { get; private set; }

        [SerializeField, RequireInterface(typeof(IStateBehaviour))]
        public List<UnityEngine.Object> stateBehaviours = new List<Object>();

        public List<IStateBehaviour> StateStateBehaviours
        {
            get
            {
                if (_stateBehaviours.Any())
                {
                    return _stateBehaviours;
                }
                
                _stateBehaviours.Clear();
                
                foreach (var behaviour in stateBehaviours.Select(stateBehaviour => stateBehaviour as IStateBehaviour))
                {
                    if (behaviour == null)
                    {
                        throw new ArgumentNullException($"{name} State does not have a valid state behaviour set");
                    }
                    
                    _stateBehaviours.Add(behaviour);
                }

                return _stateBehaviours;
            }
        }

        public bool StateRunning { get; private set; }

        protected virtual void OnEnable()
        {
            if (!stateBehaviours.Any())
            {
                throw new StateException($"State '{name}':  IState Behaviour unassigned");
            }
        }

        protected virtual void OnDisable()
        {
            if (!stateBehaviours.Any())
            {
                throw new StateException($"State '{name}':  IState Behaviour unassigned");
            }
        }

        public override async Task Enter()
        {
            StateRunning = true;
            var enterTasks = PrepareTasks(StateStateBehaviours, StateTaskSwitch.Enter);
            await Execute(enterTasks, OnEnterState);
        }

        public override async Task Idle()
        {
            StateRunning = true;
            var idleTasks = PrepareTasks(StateStateBehaviours, StateTaskSwitch.Idle);
            await Execute(idleTasks, OnIdleState);
        }

        public override async Task Exit()
        {
            var exitTasks = PrepareTasks(StateStateBehaviours, StateTaskSwitch.Idle);
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
        
        private static async Task Execute(IEnumerable<Task> tasks, Action action)
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