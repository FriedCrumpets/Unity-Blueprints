using Newtonsoft.Json;

namespace Blueprints.Http
{
    public class HttpResponse
    {
        public HttpResponse(string data)
        {
            Data = data;
        }

        public string Data { get; }

        public T Result<T>() => JsonConvert.DeserializeObject<T>(Data);
    }
}