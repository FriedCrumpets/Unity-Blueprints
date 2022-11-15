using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blueprints.StateMachine.Core;
using RequireInterface;
using UnityEngine;

namespace Blueprints.StateMachine.Mono
{
    public abstract class MonoState<TState> : State<TState> where TState : Enum 
    {
        [field: SerializeField] public TState State { get; private set; }
        
        [SerializeField, RequireInterface(typeof(IStateBehaviour))]
        private UnityEngine.Object behaviour;

        public IStateBehaviour Behaviour
        {
            get
            {
                var _ = behaviour as IStateBehaviour;
                if (_ == null)
                {
                    throw new ArgumentNullException($"{name} State does not have a valid state behaviour set");
                }

                return _;
            }   
        } 
        
        public bool StateRunning { get; private set; }

        protected virtual void OnEnable()
        {
            if (behaviour == null)
            {
                throw new StateException($"State '{name}':  IState Behaviour unassigned");
            }
        }

        protected virtual void OnDisable()
        {
            if (behaviour == null)
            {
                throw new StateException($"State '{name}':  IState Behaviour unassigned");
            }
        }

        public override async Task Enter()
        {
            StateRunning = true;
            var tasks = PrepareTasks(StateTaskSwitch.Enter);
            await Execute(tasks, OnEnterState);
        }

        public override async Task Idle()
        {
            StateRunning = true;
            var tasks = PrepareTasks(StateTaskSwitch.Idle);
            await Execute(tasks, OnIdleState);
        }

        public override async Task Exit()
        {
            var tasks = PrepareTasks(StateTaskSwitch.Exit);
            await Execute(tasks, OnExitState);
            StateRunning = false;
        }

        private IEnumerable<Task> PrepareTasks(StateTaskSwitch taskSwitch)
        {
            var task = SelectStateTask(Behaviour, taskSwitch);
            var wait = SelectStateTime(Behaviour, taskSwitch);

            return new List<Task>
            {
                task,
                Task.Delay((int)wait * 1000)
            };
        }
    }
}