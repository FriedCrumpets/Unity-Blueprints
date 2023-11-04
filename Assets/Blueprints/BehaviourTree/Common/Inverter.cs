using System;

namespace Blueprints.BehaviourTree
{
    /// <summary>
    /// Inverts the result of a child node, unless running. Running will always return running
    /// </summary>
    public class Inverter : Decorator
    {
        public Inverter(INode node) : base(node) { }

        public override Result Execute()
            => Child.Execute() switch {
                Result.Running => Result.Running,
                Result.Success => Result.Failure,
                Result.Failure => Result.Success,
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}