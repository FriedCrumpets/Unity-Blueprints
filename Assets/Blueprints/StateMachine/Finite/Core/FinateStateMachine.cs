using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Blueprints.StateMachine.Finite.Core
{
    public abstract class FiniteStateMachine<T> : MonoBehaviour
    {
        [field: SerializeField] public T Component { get; private set; }
        public IFiniteState<T> CurrentState { get; private set; }

        public virtual void HandleInput(InputAction.CallbackContext context)
        {
            var newState = CurrentState.InputHandler(Component);

            if (newState == null)
            {
                return;
            }
            
            CurrentState.Exit(Component);
            CurrentState = newState;
            newState.Enter(Component);
        }

        public virtual void Update()
        {
            CurrentState.Update(Component);
        }
    }
}