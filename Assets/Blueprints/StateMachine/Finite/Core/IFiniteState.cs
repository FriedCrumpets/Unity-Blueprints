using UnityEngine.InputSystem;

namespace Blueprints.StateMachine.Finite.Core
{
    public interface IFiniteState<in T>
    {
        public IFiniteState<T> InputHandler(InputAction.CallbackContext context);

        public void Enter(T component);

        public void Exit(T component);
        
        public void Update(T component);
    }
}