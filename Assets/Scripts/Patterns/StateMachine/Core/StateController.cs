using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Patterns.StateMachine.Core
{
    public abstract class StateController<TState> : MonoBehaviour where TState : Enum
    {
        [field: SerializeField] 
        public List<StateBehaviour<TState>> States { get; private set; }
        
        public StateBehaviour<TState> CurrentState { get; private set; }
        
        public virtual async void ChangeState(TState state)
        {
            await CurrentState.Exit();
            
            var newState = FindState(States, state);
            if (newState == null) { return; }
            CurrentState = newState;

            await CurrentState.Enter();
            await CurrentState.Idle();
        }

        private static StateBehaviour<TState> FindState(List<StateBehaviour<TState>> states, TState state)
        {
            var newState = states.First(item => item.State.Equals(state));

            if (newState == null)
            {
                throw new StateException($"StateBehaviour using '{state}' has not been found");
            }

            return newState;
        }
    }
}