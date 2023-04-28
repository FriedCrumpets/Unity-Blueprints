using Blueprints.IO.Behaviours;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Blueprints.IO.Examples
{
    public class TransformSaver : SaveableBehaviour
    {
        // Keys for the transform
        private const string _LOCAL_POSITION_KEY = "localPosition";
        private const string _LOCAL_ROTATION_KEY = "localRotation";
        private const string _LOCAL_SCALE_KEY = "localScale";

        public override JObject SavedData =>
            new()
            {
                // store values in result
                { _LOCAL_POSITION_KEY, JToken.FromObject(transform.localPosition) },
                { _LOCAL_ROTATION_KEY, JToken.FromObject(transform.localRotation) },
                { _LOCAL_SCALE_KEY, JToken.FromObject(transform.localScale) }
            };

        public override void LoadFromData(JObject data)
        {
            // we need to assume that 'data' does not contain all of the data for the object
            if (data.ContainsKey(_LOCAL_POSITION_KEY))
            {
                var value =  data.GetValue(_LOCAL_POSITION_KEY);
                if (value != null)
                    transform.localPosition = value.ToObject<Vector3>();
            }
            
            if (data.ContainsKey(_LOCAL_ROTATION_KEY))
            {
                var value =  data.GetValue(_LOCAL_ROTATION_KEY);
                if (value != null)
                    transform.localRotation = value.ToObject<Quaternion>();
            }
            
            if (data.ContainsKey(_LOCAL_SCALE_KEY))
            {
                var value =  data.GetValue(_LOCAL_SCALE_KEY);
                if (value != null)
                    transform.localScale = value.ToObject<Vector3>();
            }
        }
    }
}