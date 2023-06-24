

// https://www.gamedeveloper.com/programming/behavior-trees-for-ai-how-they-work
namespace Blueprints.BehaviourTree
{
    public enum Result : byte
    {
        Running, Success, Failure
    }   
    
    public interface INode
    {
        Result Execute();
    }

    // One Child
    // transform result of child, repeat result until desired, terminate, invert... and so on... 
    public abstract class Decorator : INode
    {
        public Decorator(INode node)
        {
            Child = node;
        }
        
        protected INode Child { get; }

        public abstract Result Execute();
    }

    // The end of a tree
    public interface ILeaf : INode  { }
}