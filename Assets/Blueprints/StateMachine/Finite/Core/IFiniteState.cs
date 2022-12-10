namespace Blueprints.StateMachine.Finite.Core
{
    public interface IFiniteState<T>
    {
        public IFiniteState<T> InputHandler(T component);

        public void Enter(T component);

        public void Exit(T component);
        
        public void Update(T component);
    }
}