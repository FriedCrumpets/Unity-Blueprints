namespace Blueprints.FSM
{
    public interface IFiniteState<in T>
    {
        public IFiniteState<T> InputHandler<TInput>(TInput input);

        public void Enter(T component);

        public void Exit(T component);
        
        public void Update(T component);
    }
}