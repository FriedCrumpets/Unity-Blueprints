using System;
using System.Collections.Generic;
using Blueprints.Core;

namespace Blueprints.Utility
{
    public class MainThreadDispatcher : MonoSingleton<MainThreadDispatcher>
    {
        private Queue<Action> ExecutionQueue { get; set; } = new();

        private void Update()
        {
            lock(ExecutionQueue)
                if( ExecutionQueue.Count > 0 )
                    ExecutionQueue.Dequeue()();
        }

        public void QueueAction(Action action)
        {
            lock(ExecutionQueue)
                ExecutionQueue.Enqueue(() => StartCoroutine(CoroutineUtils.Do(action)));
        }

        public Action<bool, string> QueueActionWithResponse(Action action)
        {
            Action<bool, string> response = (success, message) => { };
            
            QueueAction(() =>
            {
                try
                {
                    action();
                    response.Invoke(true, string.Empty);
                }
                catch(Exception e)
                {
                    response.Invoke(false, e.Message);
                }
            });

            return response;
        }
    }
}