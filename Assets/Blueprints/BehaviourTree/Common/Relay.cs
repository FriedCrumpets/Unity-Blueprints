using System;

namespace Blueprints.BehaviourTree
{
    /// <summary>
    /// Repeats chil node; will repeat infinitely until <see cref="Result"/> is Failure or <see cref="Kill"/>
    /// is called.
    /// </summary>
    public class Relay : Decorator
    {
        private bool kill;
        
        public Relay(INode node) : base(node) { }

        public override Result Execute()
        {
            if (kill)
                return Result.Failure;
            
            return Child.Execute() switch
            {
                Result.Running => Result.Running,
                Result.Success => Result.Running,
                Result.Failure => Result.Success,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public void Kill()
            => kill = true;
    }
}