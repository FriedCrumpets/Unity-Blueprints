using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Blueprints.StateMachine.Core
{
    public abstract class StateMachine<TState> : MonoBehaviour where TState : Enum
    {
        public bool StateChanging { get; protected set; }
        
        [field: SerializeField] 
        public List<State<TState>> States { get; private set; }

        public Dictionary<TState, State<TState>> AvailableStates { get; protected set; } = new Dictionary<TState, State<TState>>();

        public State<TState> CurrentState { get; protected set; }

        protected virtual void Start() => RebuildDict();

        public abstract void ChangeState(TState state);

        protected abstract void ChangeStateAsync(State<TState> newState);

        protected void RebuildDict()
        {
            foreach (var state in States)
            {
                if (AvailableStates.ContainsKey(state.CommandingState))
                {
                    throw new StateException($"Multiple States using {state.CommandingState} found in state machine: {name}");
                }
                
                AvailableStates.Add(state.CommandingState, state);
            }
        }
        
        public static State<TState> FindState(IEnumerable<State<TState>> states, TState state)
        {
            var newState = states.First(item => item.CommandingState.Equals(state));

            if (newState == null)
            {
                throw new StateException($"State using '{state}' has not been found");
            }

            return newState;
        }

        protected static State<TState> GetNewState(Dictionary<TState, State<TState>> states, TState state)
        {
            if (!states.TryGetValue(state, out var newState))
            {
                throw new StateException($"{state} has not been located within this StateMachine");
            }

            return newState;
        }
    }
}