using System;
using System.Collections.Generic;

namespace Blueprints.Scroller
{
    public class UIUpdateQueue : IQueue
    {
        private readonly Queue<Action> _updates = new Queue<Action>();

        public void Enqueue(Action update)
            => _updates.Enqueue(update);

        public Action TryDequeue()
            => _updates.TryDequeue(out var update) ? update : null;
    }

    public interface IQueue
    {
        void Enqueue(Action action );
        Action TryDequeue();
    }
}