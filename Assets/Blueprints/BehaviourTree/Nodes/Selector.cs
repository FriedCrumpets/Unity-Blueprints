using System;

namespace Blueprints.BehaviourTree.Nodes
{
    public class Selector : Composite
    {
        private int _currentNode;
        
        public Selector(params INode[] nodes) : base(nodes)
        {
            _currentNode = 0;
        }
        
        public override Result Execute()
        {
            if (_currentNode < Children.Count)
            {
                return Children[_currentNode].Execute() switch
                {
                    Result.Running => Result.Running,
                    Result.Success => Success(),
                    Result.Failure => Failure(),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            return Result.Success;
        }
        
        private Result Success()
        {
            _currentNode = 0;
            return Result.Success;
        }
        
        private Result Failure()
        {
            if(++_currentNode < Children.Count)
                return Result.Running;

            _currentNode = 0;
            return Result.Failure;
        }
    }
}