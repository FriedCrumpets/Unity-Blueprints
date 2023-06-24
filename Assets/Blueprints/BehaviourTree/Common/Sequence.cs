using System;

namespace Blueprints.BehaviourTree
{
    public class Sequence : Composite
    {
        private int _currentNode;
        
        public Sequence(params INode[] nodes) : base(nodes)
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
            if(++_currentNode < Children.Count)
                return Result.Running;

            _currentNode = 0;
            return Result.Success;
        }
        
        private Result Failure()
        {
            _currentNode = 0;
            return Result.Failure;
        }
    }
}