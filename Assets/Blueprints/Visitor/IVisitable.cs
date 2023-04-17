namespace Blueprints.Visitor
{
    public interface IVisitable<out T>
    {
        void Accept(IVisit<T> visitor);
    }
}