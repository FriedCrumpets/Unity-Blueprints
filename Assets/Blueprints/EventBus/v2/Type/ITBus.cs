using System;

namespace Blueprints.EventBus
{
    public interface ITBus
    {
        IDisposable Subscribe<T>(Action<object> observer);
        void Publish<T>(object message);
    }
}