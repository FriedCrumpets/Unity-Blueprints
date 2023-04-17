using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Blueprints.StateController
{
    public abstract class StateMachine<TState> : MonoBehaviour where TState : Enum
    {
        public bool StateChanging { get; protected set; }
        
        [field: SerializeField] 
        public List<State<TState>> States { get; private set; }

        public Dictionary<TState, State<TState>> AvailableStates { get; } = new();

        public State<TState> CurrentState { get; protected set; }

        protected virtual void Start() => RebuildDict();

        public abstract void ChangeState(TState state);

        protected abstract IEnumerator ChangeStateEnum(State<TState> newState);

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
                throw new NullReferenceException($"State using '{state}' has not been found");
            }

            return newState;
        }

        protected static State<TState> FindState(Dictionary<TState, State<TState>> states, TState state)
        {
            if (!states.TryGetValue(state, out var newState))
            {
                throw new StateException($"{state} has not been located within this StateMachine");
            }

            return newState;
        }
    }
}