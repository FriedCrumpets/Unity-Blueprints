using System;

namespace Blueprints.BehaviourTree
{
    /// <summary>
    /// Always returns a success once the child node has finish running 
    /// </summary>
    public class Succeeder : Decorator
    {
        public Succeeder(INode node) : base(node) { }

        public override Result Execute()
            => Child.Execute() switch
            {
                Result.Running => Result.Running,
                Result.Success => Result.Success,
                Result.Failure => Result.Success,
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}