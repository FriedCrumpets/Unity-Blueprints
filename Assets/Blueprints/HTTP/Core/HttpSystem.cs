using System;
using System.Collections;
using Blueprints.Core;
using Blueprints.EventBus;
using Blueprints.SystemFactory;
using UnityEngine.Networking;

namespace Blueprints.Http
{
    public class HttpSystem : MonoSingleton<HttpSystem>, ISystem<Entity>
    {
        private HttpBuffer _buffer;

        protected override void Awake()
        {
            base.Awake();
            _buffer = new HttpBuffer();
        }

        public void Init(Entity entity)
        {
            var bus = new TBus();
            bus.Subscribe<HttpRequest>(Queue);
            Transport.AddBus(typeof(HttpRequest), bus);
            
            StartCoroutine(DequeueBuffer());
        }
        
        private void Queue(object message)
        {
            var request = (HttpRequest)message;
            if(request != null)
                _buffer.QueueRequest(request);
        }

        private IEnumerator DequeueBuffer()
        {
            while (true)
            {
                var requests = _buffer.DeQueueRequests();
                if( requests.Count < 1 )
                {
                    _buffer.RequestQueued += Reboot;
                    break;
                }
                
                foreach (var request in _buffer.DeQueueRequests())
                {
                    StartCoroutine(Send(request));
                    yield return null;
                }
            }
        }
        
        private void Reboot()
        {
            StartCoroutine(DequeueBuffer());
            _buffer.RequestQueued -= Reboot;
        }

        public void Deinit()
        {
            StopAllCoroutines();
            _buffer.Dispose();
        }
        
        private static IEnumerator Send(HttpRequest request)
        {
            var numerator = request.Method switch
            {
                HttpMethod.GET => Get(request),
                HttpMethod.PUT => Put(request),
                HttpMethod.PATCH => Patch(request),
                HttpMethod.POST => Post(request),
                HttpMethod.DELETE => Delete(request),
                HttpMethod.HEAD => Head(request),
                HttpMethod.TRACE => Put(request),
                _ => throw new ArgumentOutOfRangeException()
            };

            yield return numerator;
        }
        
        private static IEnumerator Get(HttpRequest request)
        {
            var uri = request.URL;
                
            if( request.QueryParameters.Count > 0 )
            {
                uri += "?";
                foreach (var query in request.QueryParameters)
                    uri += $"{query.Key}={query.Value}";
            }

            using( var www = UnityWebRequest.Get(uri) )
                yield return Send(www, request);
        }
        
        private static IEnumerator Put(HttpRequest request)
        {
            using(var www = UnityWebRequest.Put(request.URL, request.BodyRaw))
                yield return Send(www, request);
        }

        private static IEnumerator Patch(HttpRequest request)
        {
            using(var www = UnityWebRequest.Put(request.URL, request.BodyRaw))
                yield return Send(www, request);
        }

        private static IEnumerator Post(HttpRequest request)
        {
            using( var www = new UnityWebRequest(request.URL))
            {
                www.downloadHandler = new DownloadHandlerBuffer();
                www.uploadHandler = new UploadHandlerRaw(request.BodyRaw);
                yield return Send(www, request);
            }
        }

        private static IEnumerator Delete(HttpRequest request)
        {
            using( var www = UnityWebRequest.Delete(request.URL) )
                yield return Send(www, request);
        }

        private static IEnumerator Head(HttpRequest request)
        {
            using( var www = UnityWebRequest.Head(request.URL) )
                yield return Send(www, request);
        }
        
        private static IEnumerator Send(UnityWebRequest www, HttpRequest request)
        {
            var retries = 0;
            
            www.SetRequestHeader("Content-Type", request.ContentType);
            foreach(var header in request.Headers)
                www.SetRequestHeader(header.Key, header.Value);

            www.timeout = request.TimeoutSeconds;
            www.method = request.Method.ToString();

            while (www.result != UnityWebRequest.Result.Success && retries < request.Retries)
            {
                yield return www.SendWebRequest();
                
                switch(www.result)
                {
                    case UnityWebRequest.Result.InProgress:
                        request.DownloadProgress?.Invoke(www.downloadProgress);
                        request.UploadProgress?.Invoke(www.uploadProgress);
                        break;
                    case UnityWebRequest.Result.Success:
                        request.Success?.Invoke(www.downloadHandler.data);
                        break;
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.ProtocolError:
                        retries++;
                        request.Failure?.Invoke($"Attempting Repeat:\r\n{www.error}");
                        break;
                    case UnityWebRequest.Result.DataProcessingError:
                        request.Failure?.Invoke(www.error);
                        break;
                }
            }
        }
    }
}