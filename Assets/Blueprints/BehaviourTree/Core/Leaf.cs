using System;

namespace Blueprints.BehaviourTree
{
    //todo: this class has a lot of protected bits... I don't like that... fix this to be more compositionally organised
    // ILeaf, and Branch created from this idea. Can be used interchangably 
    public abstract class Leaf : INode
    {
        protected readonly Tree tree;

        protected Leaf(Tree tree)
        {
            this.tree = tree;
        }
        
        /// <summary>
        /// If this bool is true the Leaf requires initialisation
        /// </summary>
        protected abstract bool RequireInitialisation { get; }
        
        protected abstract Result Init();
        protected abstract Result Process();
        
        public Result Execute()
        {
            if (RequireInitialisation)
            {
                if (Init() == Result.Failure)
                    return Result.Failure;
            }

            return Process();
        }

        protected static Result Process(object obj, params Func<object, Result>[] processes)
        {
            foreach (var process in processes)
            {
                var result = process(obj);
                if (result != Result.Running)
                    return result;
            }

            return Result.Failure;
        }
    }
}