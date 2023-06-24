using System;

namespace Blueprints.BehaviourTree
{
    public interface ILeaf
    {
        Tree Root { set; }
        bool RequireInitialisation { get; }
        Result Init();
        Result Process();
        
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