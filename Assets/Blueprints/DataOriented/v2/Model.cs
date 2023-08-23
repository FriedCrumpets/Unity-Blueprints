using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Collections;

namespace Blueprints.DoD.v2
{
    public interface IModel : INotifyPropertyChanged
    {
        T Read<T>(string key);
        bool Set<T>(string key, T value);
    }

    [Serializable]
    public struct Model : IModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        private Dictionary<string, object> Data { get; }

        public Model(params KeyValue<string, object>[] data)
        {
            Data = new Dictionary<string, object>();

            foreach (var pair in data)
                Data.Add(pair.Key, pair.Value);
            
            PropertyChanged = null;
        }

        T IModel.Read<T>(string key)
            => Data.TryGetValue(key, out var data) ? ((IData<T>)data).Get() : default;
        
        bool IModel.Set<T>(string key, T value)
        {
            if(Data.TryGetValue(key, out var data) is var check);
            {
                ((IData<T>)data)?.Set(value);
                OnPropertyChanged(key);
            }

            return check;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}