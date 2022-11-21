using Collections;

namespace Blueprints.Command
{
    /// <summary>
    /// A Cursor controlling the overall commands in the stream, commands can be freely moved forward and back
    /// undo-ing the previous command 
    /// </summary>
    public static class CommandStream
    {
        public static Cursor<ICommand> Cursor { get; } = new Cursor<ICommand>();

        public static ICommand CurrentCommand => Cursor.CurrentItem;
        
        public static void Execute(ICommand command)
        {
            command.Execute();
            
            if (Cursor.Location != Cursor.Count)
            {
                Cursor.ClearFromLocation();    
            }
            
            Cursor.Add(command);
        }

        public static void Undo()
        {
            CurrentCommand.Undo();
            Cursor.RemoveAt((int)Cursor.Location);
        }
    }
}