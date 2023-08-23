using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Blueprints.DoD.v1
{
    [Serializable]
    public class DataObject
    {
        public DataObject()
        {
            data = new Data<float>(15);
        }
        
        public Data<float> data;
    }
    
    public class Test : MonoBehaviour
    {
        private DataObject data;
        private void Awake()
        {
            data = new ();
            print(JsonConvert.SerializeObject(data));
        }
    }
}