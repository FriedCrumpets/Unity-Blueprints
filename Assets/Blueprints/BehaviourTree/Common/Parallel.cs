
namespace Blueprints.BehaviourTree
{
    public class Parallel : Composite
    {
        public Parallel(params INode[] nodes) : base(nodes) { }
        
        public override Result Execute()
        {
            foreach (var child in Children)
            {
                var compare = child.Execute();
                
                if (Result.Running != compare)
                    return compare;
            }

            return Result.Running;
        }
    }
}