namespace Blueprints.Entities
{
    public interface ISystem<in T>
    {
        void Init(T entity);
        void Deinit();
    }
}