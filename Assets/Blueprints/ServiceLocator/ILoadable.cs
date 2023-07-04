namespace Blueprints.Utility
{
    public interface ILoadable : IService
    {
        void Save();
        void Load();
    }
}