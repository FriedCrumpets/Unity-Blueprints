using System;

namespace Blueprints.StateMachine.Finite.Core
{
    public class FiniteState<T, TState> : IFiniteState<T> where TState : Enum
    {
        public FiniteState(TState state)
        {
            State = state;
        }

        public FiniteState(TState state, Func<T, IFiniteState<T>> handleInput, Action<T> handleUpdate)
        {
            State = state;
            HandleInput = handleInput;
            HandleUpdate = handleUpdate;
        }

        public Func<T, IFiniteState<T>> HandleInput { get; }
        public Action<T> HandleUpdate { get; }

        public TState State { get; }
        
        public virtual IFiniteState<T> InputHandler(T component) { return HandleInput?.Invoke(component); }
        
        public virtual void Enter(T component) { }

        public void Exit(T component) { }

        public virtual void Update(T component) { HandleUpdate?.Invoke(component); }
    }
}