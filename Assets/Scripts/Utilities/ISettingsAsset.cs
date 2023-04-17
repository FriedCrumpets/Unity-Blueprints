namespace Core.Utils
{
    public interface ISettingsAsset
    {
        void Save();
        void Load();
        void Refire();
    }
}