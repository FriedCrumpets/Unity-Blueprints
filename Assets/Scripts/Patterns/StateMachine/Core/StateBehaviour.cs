using System;
using System.Threading.Tasks;
using RequireInterface;
using UnityEngine;

namespace Patterns.StateMachine.Core
{
    public abstract class StateBehaviour<TState> : MonoBehaviour where TState : Enum
    {
        public event Action EnterState;
        public event Action IdleState;
        public event Action ExitState;

        [field: SerializeField] public TState State { get; private set; }
        
        [SerializeField, RequireInterface(typeof(IState))]
        private UnityEngine.Object behaviour;
        
        public IState Behaviour => behaviour as IState; 
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

        public async Task Enter()
        {
            StateRunning = true;
            await Execute(EnterState, Behaviour.EnterTime);
        }

        public async Task Idle()
        {
            StateRunning = true;
            await Execute(IdleState, Behaviour.IdleTime);
        }

        public async Task Exit()
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