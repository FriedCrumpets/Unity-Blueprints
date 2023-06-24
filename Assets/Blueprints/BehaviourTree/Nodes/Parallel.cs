
namespace Blueprints.BehaviourTree.Nodes
{
    public class Parallel : Composite
    {
        private int _currentNode;
        
        public Parallel(params INode[] nodes) : base(nodes)
        {
            _currentNode = 0;
        }
        
        public override Result Execute()
        {
            var stable = Result.Running;
            foreach (var child in Children)
            {
                var compare = child.Execute();
                
                if (stable != compare)
                    return compare;
            }

            return Result.Running;
        }
    }
}