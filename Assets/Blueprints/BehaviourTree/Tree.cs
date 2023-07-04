using System.Collections;
using System.Collections.Generic;

namespace Blueprints.BehaviourTree
{
    public class Tree
    {
        public Tree(INode root)
        {
            BlackBoard = new Dictionary<string, object>();
            Root = root;
        }
        
        public Dictionary<string, object> BlackBoard { get; }
        public INode Root { get; }

        public object Add(string key, object obj)
        {
            BlackBoard.Add(key, obj);
            return obj;
        }

        public T Add<T>(string key, T obj)
        {
            BlackBoard.Add(key, obj);
            return obj;
        }

        public object TryGet(string key)
            => BlackBoard.TryGetValue(key, out var obj) ? obj : null;
        
        public T TryGet<T>(string key)
            => BlackBoard.TryGetValue(key, out var obj) ? (T)obj : default;

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