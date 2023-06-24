using System.Collections;
using System.Collections.Generic;

namespace Blueprints.BehaviourTree
{
    public class Tree
    {
        public Tree(INode root = null)
        {
            BlackBoard = new Dictionary<string, object>();
            Root = root;
        }
        
        public Dictionary<string, object> BlackBoard { get; }
        public INode Root { get; }
    
        public IEnumerator Run()
        {
            var result = Root.Execute();
            while (result == Result.Running)
            {
                yield return null;
                result = Root.Execute();
            }
        }
    }
}