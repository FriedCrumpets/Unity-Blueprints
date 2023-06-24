using System.Collections;
using UnityEngine;

namespace Blueprints.BehaviourTree
{
    public class PopFromStack : ILeaf
    {
        private readonly string _stackLocation;
        private readonly string _popLocation;

        private Stack _stack;

        public PopFromStack(string stackLocation, string popLocation, Tree root = null)
        {
            _stackLocation = stackLocation;
            _popLocation = popLocation;
            Root = root;
        }
        
        public Tree Root { get; set; }

        bool ILeaf.RequireInitialisation
            => _stack is null;
        
        Result ILeaf.Init()
        {
            var item = Root.TryGet(_stackLocation);
            return ILeaf.Process(item, _IsNull, _NotStack, _IsStack);
        }

        Result ILeaf.Process()
        {
            Root.Add(_popLocation, _stack.Pop());
            return Result.Success;
        }
        
        private Result _IsNull(object obj)
        {
            if (obj is not null)
                return Result.Running;
            
            Debug.LogError($"{nameof(PushToStack)}: item: {nameof(obj)} at location: {_stackLocation} does not exist");
            return Result.Failure;
        }
        
        private Result _NotStack(object obj)
        {
            if (obj is Stack)
                return Result.Running;
            
            Debug.LogError($"{nameof(PushToStack)}: item: {obj} at location: {_stackLocation} is not of type {nameof(Stack)}");
            return Result.Failure;
        }

        private Result _IsStack(object obj)
        {
            if (obj is not Stack stack)
                return Result.Failure;
            
            _stack = stack;
            return Result.Success;
        }
    }
}