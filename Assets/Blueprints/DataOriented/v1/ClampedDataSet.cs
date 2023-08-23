using System;

namespace Blueprints.DoD.v1
{
    [Serializable]
    public class ClampedDataSet : IDataSet<float>
    {
        private const string _VALUE = "value";
        private const string _MIN = "min";
        private const string _MAX = "max";

        public ClampedDataSet()
        {
            Data = new DataSet(); 
            Data.Add(_VALUE, new Data<float>());
            Data.Add(_MIN, new Data<float>());
            Data.Add(_MAX, new Data<float>());
        }
        
        public ClampedDataSet(float value, float min, float max)
        {
            Data = new DataSet();  
            Data.Add(_VALUE, new Data<float>(value));
            Data.Add(_MIN, new Data<float>(min));
            Data.Add(_MAX, new Data<float>(max));
        }
        
        public IDataSet Data { get; }

        float IDataSet<float>.Read(string key)
            => Data.Read<float>(key);

        IData<float> IDataSet<float>.Get(string key)
            => Data.Get<float>(key);

        void IDataSet<float>.Add(string key, IData<float> value)
            => Data.Add(key, value);
        
        bool IDataSet<float>.Set(string key, float value)
        {
            switch(key)
            {
                case _VALUE:
                    if (value >= Data.Read<float>(_MIN) && value <= Data.Read<float>(_MAX))
                        Data.Set(_VALUE, value); return true;
                case _MIN:
                    if (value <= Data.Read<float>(_VALUE) && value <= Data.Read<float>(_MAX))
                        Data.Set(_MIN, value); return true;
                case _MAX:
                    if (value >= Data.Read<float>(_VALUE) && value >= Data.Read<float>(_MIN))
                        Data.Set(_MIN, value); return true;
            }
            return false;
        }

        bool IDataSet<float>.Remove(string key)
            => Data.Remove(key);
    }
}