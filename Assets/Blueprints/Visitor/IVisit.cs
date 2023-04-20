namespace Blueprints.Visitor
{
    public interface IVisit<in T>
    {
        void Visit(IVisitable<T> visitable);
    }
    
    public interface IVisit
    {
        void Visit<T>(IVisitable<T> visitable);
    }
}