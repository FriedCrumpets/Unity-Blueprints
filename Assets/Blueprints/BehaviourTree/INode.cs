

// https://www.gamedeveloper.com/programming/behavior-trees-for-ai-how-they-work
// https://www.youtube.com/watch?v=aVf3awPrVPE
namespace Blueprints.BehaviourTree
{
    public enum Result : byte
    {
        Failure,
        Success, 
        Running, 
    }   
    
    public interface INode
    {
        Result Execute();
    }
}