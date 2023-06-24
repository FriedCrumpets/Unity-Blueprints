namespace Blueprints.BehaviourTree.Nodes
{
    public class Repeater : Decorator
    {
        public Repeater(INode node) : base(node)
        {
            
        }

        public override Result Execute()
        {
            return Result.Failure;
        }
    }
}