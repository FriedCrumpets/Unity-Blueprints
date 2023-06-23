using Newtonsoft.Json.Linq;

namespace Blueprints.IO
{
    public static class IOUtils
    {
        public static JObject SerializeValue(this object obj)
            => JObject.FromObject(obj);

        public static T DeserializeValue<T>(this JObject obj)
            => obj.ToObject<T>();
        
        public static T DeserializeValue<T>(this JToken obj)
            => obj.ToObject<T>();
    }
}