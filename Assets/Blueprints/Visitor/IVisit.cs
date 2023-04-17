namespace Blueprints.Visitor
{
    public interface IVisit<in T>
    {
        void Visit(IVisitable<T> visitable);
    }
}