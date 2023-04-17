using System;
using System.Collections;
using System.Collections.Generic;
using Blueprints.Attributes;
using UnityEngine;

namespace Blueprints.StateController
{
    public abstract class MonoState<TState> : State<TState> where TState : Enum 
    {
        [SerializeField, RequireInterface(typeof(IState))]
        private UnityEngine.Object behaviour;

        public IState Behaviour
        {
            get
            {
                var _ = behaviour as IState;
                if (_ == null)
                {
                    throw new ArgumentNullException($"{name} State does not have a valid state behaviour set");
                }

                return _;
            }   
        }

        protected virtual void OnEnable()
        {
            if (behaviour == null)
            {
                throw new NullReferenceException($"State '{name}':  IState Behaviour unassigned");
            }
        }

        protected virtual void OnDisable()
        {
            if (behaviour == null)
            {
                throw new NullReferenceException($"State '{name}':  IState Behaviour unassigned");
            }
        }
        
        public override IEnumerator Enter()
        {
            StateRunning = true;
            var tasks = PrepareTasks(StateTaskSwitch.Enter);
            yield return StartCoroutine(Execute(tasks, OnEnterState));
        }

        public override IEnumerator Idle()
        {
            StateRunning = true;
            var tasks = PrepareTasks(StateTaskSwitch.Idle);
            yield return StartCoroutine(Execute(tasks, OnIdleState));
        }

        public override IEnumerator Exit()
        {
            var tasks = PrepareTasks(StateTaskSwitch.Exit);
            yield return StartCoroutine(Execute(tasks, OnExitState));
            StateRunning = false;
        }

        private IEnumerable<IEnumerator> PrepareTasks(StateTaskSwitch taskSwitch)
        {
            var task = SelectStateTask(Behaviour, taskSwitch);
            var wait = SelectStateTime(Behaviour, taskSwitch);

            return new List<IEnumerator>
            {
                task,
                WaitForSeconds(wait),
            };
        }
    }
}