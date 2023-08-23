using System;
using System.Collections.Generic;
using System.Text;
using Blueprints.EventBus;
using Blueprints.EventBus.v1;
using Newtonsoft.Json;

namespace Blueprints.Http
{
    public class HttpRequest
    {
        public HttpRequest(string url, HttpRequest request)
        {
            URL = url;
            Request = request;
            Headers = new Dictionary<string, string>();
            QueryParameters = new Dictionary<string, string>();
            BodyRaw = Array.Empty<byte>();
        }
        
        public string URL { get; }
        public HttpRequest Request { get; }
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> QueryParameters { get; set; }
        public byte[] BodyRaw { get; private set; }
        public string ContentType { get; set; } = "application/json";
        public double TimeoutSeconds { get; set; } = 10.0;
        public int Retries { get; set; } = 2;
        public int Priority { get; set; } = -1;

        public void SetBodyFromJson(string json)
            => BodyRaw = Encoding.UTF8.GetBytes(json);

        public void SetBodyFromObject(object obj)
            => BodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));

        public void Send()
            => GlobalBus.Publish<HttpRequest>(this);
    }
}