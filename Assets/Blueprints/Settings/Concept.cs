namespace Blueprints.Settings
{
    [System.Serializable]
    public class Concept
    {
        public Concept_SO localStorage;

        public float Setting1
        {
            get => localStorage.Setting1;
            set
            {
                // do things
                localStorage.Setting1 = value;
            }
        }

        public string Setting2
        {
            get => localStorage.Setting2;
            set => localStorage.Setting2 = value;
        }

        public bool Setting3
        {
            get => localStorage.Setting3;
            set => localStorage.Setting3 = value;
        }
    }
}