using System;
using Blueprints.StateMachine.Core;

namespace Blueprints.StateMachine.Mono
{
    public abstract class MonoStateMachine<TState> : StateMachine<TState> where TState : Enum
    {
        public override void ChangeState(TState state)
        {
            if (StateChanging) { return; }
            
            var newState = GetNewState(AvailableStates, state);
            ChangeStateAsync(newState);
        }
        
        protected override async void ChangeStateAsync(State<TState> newState)
        {
            StateChanging = true;
            await CurrentState.Exit();
            CurrentState = newState;
            await CurrentState.Enter();
            await CurrentState.Idle();
            StateChanging = false;
        }
    }
}