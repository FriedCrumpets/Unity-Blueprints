using System.Collections;
using UnityEngine;

namespace Blueprints.BehaviourTree
{
    public sealed class PushToStack : ILeaf
    {
        private readonly object _item;
        private readonly string _location;

        private Stack _stack;

        public PushToStack(object item, string location, Tree root = null)
        {
            _item = item;
            _location = location;
            Root = root;
        }
        
        public Tree Root { get; set; }

        bool ILeaf.RequireInitialisation
            => _stack is null;

        Result ILeaf.Init()
        {
            var item = Root.TryGet(_location);
            return ILeaf.Process(item, _IsNull, _NotStack, _IsStack);
        }

        Result ILeaf.Process()
        {
            _stack.Push(_item);
            return Result.Success;
        }

        private Result _IsNull(object obj)
        {
            if (obj is not null)
                return Result.Running;
            
            _stack = Root.Add(_location, new Stack());
            return Result.Success;
        }

        private Result _NotStack(object obj)
        {
            if (obj is Stack)
                return Result.Running;
            
            Debug.LogError($"{nameof(PushToStack)}: item: {obj} at location: {_location} is not of type {nameof(Stack)}");
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