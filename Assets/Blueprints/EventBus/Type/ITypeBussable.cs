using System;

namespace Blueprints.StaticMessaging
{
    public interface ITypeBussable
    {
        IDisposable Subscribe<T>(Action<object> observer);
        void Publish<T>(object message);
    }
}