
namespace Blueprints.BehaviourTree
{
    public class Parallel : Composite
    {
        private const Result _STABLE_RESULT = Result.Running;
        
        public Parallel(params INode[] nodes) : base(nodes) { }
        
        public override Result Execute()
        {
            foreach (var child in Children)
            {
                var compare = child.Execute();
                
                if (_STABLE_RESULT != compare)
                    return compare;
            }

            return Result.Running;
        }
    }
}