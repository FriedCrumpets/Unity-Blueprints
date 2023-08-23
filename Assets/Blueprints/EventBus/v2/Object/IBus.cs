using System;

namespace Blueprints.EventBus
{
    public interface IBus
    {
        object Type { get; }
        IDisposable Subscribe(object obj, Action<object> observer);
        void Publish(object obj, object message);
    }
}