using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Blueprints.StateMachine.Core
{
    public abstract class StateMachine<TState> : MonoBehaviour where TState : Enum
    {
        [field: SerializeField] 
        public List<State<TState>> States { get; private set; }
        
        public State<TState> CurrentState { get; private set; }
        
        public virtual async void ChangeState(TState state)
        {
            await CurrentState.Exit();
            
            var newState = FindState(States, state);
            if (newState == null) { return; }
            CurrentState = newState;

            await CurrentState.Enter();
            await CurrentState.Idle();
        }

        private static State<TState> FindState(List<State<TState>> states, TState state)
        {
            var newState = states.First(item => item.CommandingState.Equals(state));

            if (newState == null)
            {
                throw new StateException($"StateBehaviour using '{state}' has not been found");
            }

            return newState;
        }
    }
}