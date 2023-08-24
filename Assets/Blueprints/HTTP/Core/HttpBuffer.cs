using System;
using System.Collections.Generic;

namespace Blueprints.Http
{
    public class HttpBuffer : IDisposable
    {
        public event Action RequestQueued;

        public HttpBuffer()
        {
            Buffers = new Dictionary<int, Queue<HttpRequest>>();
        }

        private IDictionary<int, Queue<HttpRequest>> Buffers { get; }
        
        public void QueueRequest(HttpRequest request)
        {
            if(Buffers.TryGetValue(request.Queue, out var buffer))
                buffer.Enqueue(request);
            else
                Buffers.Add(request.Queue, new Queue<HttpRequest>(new []{ request }));
            
            RequestQueued?.Invoke();
        }

        public ICollection<HttpRequest> DeQueueRequests()
        {
            var requests = new List<HttpRequest>();
            
            foreach (var buffer in Buffers)
            {
                if( buffer.Value.TryDequeue(out var request) )
                    requests.Add(request);
                else
                    Buffers.Remove(buffer);
            }

            return requests;
        }
        public void Dispose()
            => Buffers.Clear();
    }
}