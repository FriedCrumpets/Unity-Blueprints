namespace Blueprints.Command
{
    
    // TODO: implement or don't use this... I'm not sure. I think it'd be useful... need better state names
    public enum CommandState
    {
        None,
        Buffered,
        Executed,
        Undone
    }
}