namespace Blueprints.Visitor
{
    public interface IVisitable
    {
        void Accept<T>(IVisit<T> visitor);
    }
    
    public interface IVisitable<out T>
    {
        void Accept(IVisit<T> visitor);
    }
}