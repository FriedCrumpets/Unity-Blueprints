using UnityEngine;
using UnityEngine.InputSystem;

namespace Blueprints.FSM
{
    public sealed class FiniteStateMachine<T> : MonoBehaviour where T : Component
    {
        [field: SerializeField] public T Component { get; private set; }
        public IFiniteState<T> CurrentState { get; private set; }

        public void HandleInput(InputAction.CallbackContext context)
        {
            var newState = CurrentState.InputHandler(context);
            
            if (newState == null || newState == CurrentState)
            {
                return;
            }

            CurrentState.Exit(Component);
            CurrentState = newState;
            newState.Enter(Component);
        }

        public void Update() => CurrentState.Update(Component);
    }
}