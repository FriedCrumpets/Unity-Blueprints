namespace Blueprints.Utility
{
    public interface IService { }
    
    public interface ILoadable : IService
    {
        void Save();
        void Load();
    }

}