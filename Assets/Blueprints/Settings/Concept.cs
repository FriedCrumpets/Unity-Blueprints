namespace Blueprints.Settings
{
    [System.Serializable]
    public class Concept
    {
        public Concept_SO cachedStorage;

        public float Setting1
        {
            get => cachedStorage.Setting1;
            set
            {
                // do things
                cachedStorage.Setting1 = value;
            }
        }

        public string Setting2
        {
            get => cachedStorage.Setting2;
            set => cachedStorage.Setting2 = value;
        }

        public bool Setting3
        {
            get => cachedStorage.Setting3;
            set => cachedStorage.Setting3 = value;
        }
    }
}