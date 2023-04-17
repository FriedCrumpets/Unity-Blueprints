using Blueprints.Visitor;

namespace Features.Player.Consumable
{
    public abstract class Effect<T> : IVisit<T>
    {
        public abstract void Visit(IVisitable<T> visitable);
    }
}