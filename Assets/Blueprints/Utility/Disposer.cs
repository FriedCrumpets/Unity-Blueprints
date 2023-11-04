using System;

namespace Blueprints.Utility
{
    public class Disposer : IDisposable
    {
        private readonly Action _destructionAction;
        
        public Disposer(Action destructionAction)
        {
            _destructionAction = destructionAction;
        }

        public void Dispose()
            => _destructionAction?.Invoke();
    }
}