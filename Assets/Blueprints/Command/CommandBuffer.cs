using System.Collections.Generic;
using System.Linq;

namespace Blueprints
{
    /// <summary>
    /// Buffers the commands in a queue ready to be executed prior to adding them to the CommandStream
    /// Commands used in this buffer are to be tied quite heavily to GameObjects,
    /// with this in mind this Buffer might require to be of a Singleton pattern to set the clarity.
    ///
    /// To Clarify, ICommands do not need to be tied to GameObjects and if they are related to data could be used
    /// asynchronously... if that doesn't cause issues. Again I'm not sure yet. 
    /// </summary>
    public static class CommandBuffer
    {
        private static Queue<ICommand> Queue { get; set; } = new Queue<ICommand>();

        public static void Clear() 
            => Queue.Clear();
        
        public static void Buffer(ICommand command)
            => Queue.Enqueue(command);
        
        public static void Cancel(ICommand command)
            => Queue = new Queue<ICommand>(Queue.Where(x => x != command));

        public static void Execute()
            => CommandStream.Execute(Queue.Dequeue());
    }
}