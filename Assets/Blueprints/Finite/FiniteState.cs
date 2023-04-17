using System;

namespace Blueprints.FSM
{
    public abstract class FiniteState<T> : IFiniteState<T> 
    {
        public event Action<T> OnEnter;
        public event Action<T> OnExit;
        
        public abstract IFiniteState<T> InputHandler<TInput>(TInput input);
        
        public virtual void Enter(T component) => OnEnter?.Invoke(component);

        public virtual void Exit(T component) => OnExit?.Invoke(component);

        public abstract void Update(T component);
    }
}