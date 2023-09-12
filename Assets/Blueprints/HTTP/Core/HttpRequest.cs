using System;
using System.Collections.Generic;
using System.Text;
using Blueprints.StaticMessaging;
using Newtonsoft.Json;

namespace Blueprints.Http
{
    public class HttpRequest
    {
        public Action<byte[]> Success;
        public Action<float> DownloadProgress;
        public Action<float> UploadProgress;
        public Action<string> Failure;
        
        public HttpRequest(string url, HttpMethod method)
        {
            URL = url;
            Method = method;
            Headers = new Dictionary<string, string>();
            QueryParameters = new Dictionary<string, string>();
            BodyRaw = Array.Empty<byte>();
        }

        public string URL { get; }
        public HttpMethod Method { get; }
        public Dictionary<string, string> Headers { get; }
        public Dictionary<string, string> QueryParameters { get; }
        public byte[] BodyRaw { get; private set; }
        public string ContentType { get; set; } = "application/json";
        public int TimeoutSeconds { get; set; } = 10;
        public int Retries { get; set; } = 2;
        public int Queue { get; set; } = -1;
        
        public void SetBodyFromJson(string json)
            => BodyRaw = Encoding.UTF8.GetBytes(json);

        public void SetBodyFromObject(object obj)
            => BodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));

        public void Send()
        {
            var bus = Transport.RetrieveBus(typeof(HttpRequest));
            
            if( bus != default )
                bus.Publish<HttpRequest>(this);
            else
                Transport.NewBus += WaitForBus;
        }

        private void WaitForBus(object key, ITypeBussable bus)
        {
            if( (HttpRequest)key != null )
            {
                bus.Publish<HttpRequest>(this);
                Transport.NewBus -= WaitForBus;
            }
        }
    }
}