using System;

namespace Blueprints.ChainOfResponsibility
{
    public interface IHandler
    {
        bool Handle(object request);
    }
    
    public class Handler : IHandler
    {
        private readonly Func<object, bool> _check;
        private readonly Func<object, bool> _fallback;

        public Handler(Func<object, bool> check, Func<object, bool> fallback, IHandler next = null)
        {
            _check = check;
            _fallback = fallback;
            
            if (next != null)
                Next = next;
        }

        public IHandler Next { get; set; }

        public bool Handle(object request)
            => _check.Invoke(request) ? HandleNext(request) : _fallback.Invoke(request);
        
        private bool HandleNext(object request)
            => Next == null || Next.Handle(request);
    }
    
    public abstract class BaseHandler : IHandler
    {
        protected BaseHandler(IHandler next = null)
        {
            if (next != null)
                Next = next;
        }
        
        public IHandler Next { get; set; }
        
        public abstract bool Handle(object request);
        
        protected bool HandleNext(object request)
            => Next == null || Next.Handle(request);
    }
}