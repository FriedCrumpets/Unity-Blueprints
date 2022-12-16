namespace Blueprints.Crafting
{
    public interface ICraftable : IDissmantleable, IAssembleable
    {
        
    }

    public interface IDissmantleable
    {
        void Dismantle();
    }

    public interface IAssembleable
    {
        void Assemble();
    }
}