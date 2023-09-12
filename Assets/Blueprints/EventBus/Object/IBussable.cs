using System;

namespace Blueprints.StaticMessaging
{
    public interface IBussable
    {
        object Type { get; }
        IDisposable Subscribe(object obj, Action<object> observer);
        void Publish(object obj, object message);
    }
}