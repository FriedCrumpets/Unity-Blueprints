using System.Collections.Generic;
using System.Linq;

namespace Blueprints.Command
{
    /// <summary>
    /// Buffers the commands in a queue ready to be executed prior to adding them to the CommandStream
    /// </summary>
    public static class CommandBuffer
    {
        private static Queue<ICommand> Queue { get; set; } = new Queue<ICommand>();

        public static void Clear() => Queue.Clear();
        
        public static void Buffer(ICommand command)
        {
            Queue.Enqueue(command);
        }

        public static void Cancel(ICommand command)
        {
            Queue = new Queue<ICommand>(Queue.Where(x => x != command));
        }
        
        public static void Execute()
        {
            CommandStream.Execute(Queue.Dequeue());
        }
    }
}