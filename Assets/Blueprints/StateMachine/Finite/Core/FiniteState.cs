using System;
using UnityEngine.InputSystem;

namespace Blueprints.StateMachine.Finite.Core
{
    public class FiniteState<T, TState> : IFiniteState<T> where TState : Enum
    {
        public FiniteState(TState state)
        {
            State = state;
        }

        public FiniteState(TState state, Func<InputAction.CallbackContext, IFiniteState<T>> handleInput, Action<T> handleUpdate)
        {
            State = state;
            HandleInput = handleInput;
            HandleUpdate = handleUpdate;
        }

        public Func<InputAction.CallbackContext, IFiniteState<T>> HandleInput { get; }
        public Action<T> HandleUpdate { get; }

        public TState State { get; }
        
        public IFiniteState<T> InputHandler(InputAction.CallbackContext context) => HandleInput?.Invoke(context);
        
        public virtual void Enter(T component) { }

        public void Exit(T component) { }

        public virtual void Update(T component) { HandleUpdate?.Invoke(component); }
    }
}