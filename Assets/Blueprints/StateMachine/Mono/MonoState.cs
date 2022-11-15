using System;
using System.Threading.Tasks;
using Blueprints.StateMachine.Core;
using RequireInterface;
using UnityEngine;

namespace Blueprints.StateMachine.Mono
{
    public abstract class MonoState<TState> : State<TState> where TState : Enum 
    {
        public event Action EnterState;
        public event Action IdleState;
        public event Action ExitState;

        [field: SerializeField] public TState State { get; private set; }
        
        [SerializeField, RequireInterface(typeof(IStateBehaviour))]
        private UnityEngine.Object behaviour;
        
        public IStateBehaviour Behaviour => behaviour as IStateBehaviour; 
        public bool StateRunning { get; private set; }

        protected void OnEnable()
        {
            if (behaviour == null)
            {
                throw new StateException($"State '{name}':  IState Behaviour unassigned");
            }

            EnterState += Behaviour.Enter;
            IdleState += Behaviour.Idle;
            ExitState += Behaviour.Exit;
        }

        protected void OnDisable()
        {
            if (behaviour == null)
            {
                throw new StateException($"State '{name}':  IState Behaviour unassigned");
            }
            
            EnterState -= Behaviour.Enter;
            IdleState -= Behaviour.Idle;
            ExitState -= Behaviour.Exit;
        }

        public override async Task Enter()
        {
            StateRunning = true;
            await Execute(EnterState, Behaviour.EnterTime);
        }

        public override async Task Idle()
        {
            StateRunning = true;
            await Execute(IdleState, Behaviour.IdleTime);
        }

        public override async Task Exit()
        {
            await Execute(ExitState, Behaviour.ExitTime);
            StateRunning = false;
        }

        private static async Task Execute(Action stateAction, float waitForSeconds)
        {
            stateAction?.Invoke();
            await Task.Delay((int)(waitForSeconds * 1000));
        }
    }
}