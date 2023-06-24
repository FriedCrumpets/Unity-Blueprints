using System.Collections.Generic;

namespace Blueprints.BehaviourTree
{
    // can have any number of children (use params) 
    // during the time they are processing children they return running 
    public abstract class Composite : INode
    {
        public Composite(params INode[] nodes)
        {
            Children = new List<INode>(nodes);
        }
        
        protected List<INode> Children { get; }
        
        public abstract Result Execute();
    }
}