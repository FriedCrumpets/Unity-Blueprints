using System;

namespace Blueprints.DoD
{
    [Serializable]
    public class ClampedDataSet : IDataSet<float>
    {
        public ClampedDataSet()
        {
            DataSet = new DataSet<float>(); 
            DataSet.Add("value", new Data<float>());
            DataSet.Add("min", new Data<float>());
            DataSet.Add("max", new Data<float>());
        }
        
        public ClampedDataSet(float value, float min, float max)
        {
            DataSet = new DataSet<float>();  
            DataSet.Add("value", new Data<float>(value));
            DataSet.Add("min", new Data<float>(min));
            DataSet.Add("max", new Data<float>(max));
        }
        
        private IDataSet<float> DataSet { get; }
        
        float IDataSet<float>.Read(string key)
            => DataSet.Read(key);

        IData<float> IDataSet<float>.Get(string key)
            => DataSet.Get(key);

        void IDataSet<float>.Add(string key, IData<float> value)
            => DataSet.Add(key, value);
        
        bool IDataSet<float>.Set(string key, float value)
        {
            switch(key)
            {
                case "value":
                    if (value >= DataSet.Read("min") && value <= DataSet.Read("max"))
                        DataSet.Set("value", value); return true;
                case "min":
                    if (value <= DataSet.Read("value") && value <= DataSet.Read("max"))
                        DataSet.Set("min", value); return true;
                case "max":
                    if (value >= DataSet.Read("value") && value >= DataSet.Read("min"))
                        DataSet.Set("min", value); return true;
            }
            return false;
        }

        bool IDataSet<float>.Remove(string key)
            => DataSet.Remove(key);
    }
}