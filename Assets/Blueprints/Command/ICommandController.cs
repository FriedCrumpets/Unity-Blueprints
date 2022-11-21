using System;

namespace Blueprints.Command
{
    public interface ICommandController : IDisposable
    {
        public void Prepare();
        public void Execute();
        public void Interrupt();
        public void Undo();
    }
}