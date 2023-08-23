using System;
using System.Collections;
using System.Collections.Generic;
using Blueprints.EventBus;
using UnityEngine.Networking;

namespace Blueprints.Http
{
    public class HttpBuffer
    {
        public event Action RequestQueued;

        public HttpBuffer()
        {
            Buffers = new Dictionary<int, Queue<HttpRequest>>();
            GlobalBus.Subscribe<HttpRequest>();
        }
        
        private void Awake()
        {
            
        }

        public IDictionary<int, Queue<HttpRequest>> Buffers { get; }

        private void Queue(object request)
            => QueueRequest((HttpRequest)request);
        
        public void QueueRequest(HttpRequest request)
        {
            if(Buffers.TryGetValue(request.Priority, out var buffer))
                buffer.Enqueue(request);
            else
                Buffers.Add(request.Priority, new Queue<HttpRequest>(new []{ request }));
            
            RequestQueued?.Invoke();
        }
        
        public IEnumerator Get(HttpRequest request)
        {
            
        }

        public IEnumerator Put(HttpRequest request)
        {
            
        }

        public IEnumerator Patch(HttpRequest request)
        {
            
        }

        public IEnumerator Post(HttpRequest request)
        {
            var uRequest = new UnityWebRequest(request.URL, "POST");
            uRequest.uploadHandler = new UploadHandlerRaw(request.BodyRaw);
            uRequest.downloadHandler = new DownloadHandlerBuffer();
            uRequest.SetRequestHeader("Content-Type", request.ContentType);
            
            foreach(var header in request.Headers)
                uRequest.SetRequestHeader(header.Key, header.Value);
        }

        public IEnumerator Delete(HttpRequest request)
        {
            
        }

        public IEnumerator Head(HttpRequest request)
        {
            
        }

        public IEnumerator Options(HttpRequest request)
        {
            
        }

        public IEnumerator Connect(HttpRequest request)
        {
            
        }

        public IEnumerator Trace(HttpRequest request)
        {
            
        }
    }
}