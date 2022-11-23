using System;
using System.Collections.Generic;
using UnityEngine;

namespace Blueprints.Observer
{
    public class GameEvent<T> : ScriptableObject
    {
        protected readonly List<Action<T>> Responses = new List<Action<T>>();

        public virtual void Raise(T args)
        {
            Responses.ForEach(action => action.Invoke(args));
        }

        private void Register(Action<T> response)
        {
            if (Responses.Contains(response))
            {
                throw new ArgumentException($"Cannot register a response to the same GameEvent twice");
            }
            
            Responses.Add(response);
        }

        private void Remove(Action<T> response)
        {
            Responses.Remove(response);
         }
        
        public static GameEvent<T> operator +(GameEvent<T> gameEvent, Action<T> response)
        {
            gameEvent.Register(response);
            return gameEvent;
        }

        public static GameEvent<T> operator -(GameEvent<T> gameEvent, Action<T> response)
        {
            gameEvent.Remove(response);
            return gameEvent;
        }
    }

    public class GameEvent<T1, T2> : ScriptableObject
    {
        protected readonly List<Action<T1, T2>> Responses = new List<Action<T1, T2>>();
        
        private void Register(Action<T1, T2> response)
        {
            if (Responses.Contains(response))
            {
                throw new ArgumentException($"Cannot register a response to the same GameEvent twice");
            }
            
            Responses.Add(response);
        }

        private void Remove(Action<T1, T2> response)
        {
            Responses.Remove(response);
         }
        
        public static GameEvent<T1, T2> operator +(GameEvent<T1, T2> gameEvent, Action<T1, T2> response)
        {
            gameEvent.Register(response);
            return gameEvent;
        }

        public static GameEvent<T1, T2> operator -(GameEvent<T1, T2> gameEvent, Action<T1, T2> response)
        {
            gameEvent.Remove(response);
            return gameEvent;
        }
    }
    
    public class GameEvent<T1, T2, T3> : ScriptableObject
    {
        protected readonly List<Action<T1, T2, T3>> Responses = new List<Action<T1, T2, T3>>();
        
        private void Register(Action<T1, T2, T3> response)
        {
            if (Responses.Contains(response))
            {
                throw new ArgumentException($"Cannot register a response to the same GameEvent twice");
            }
            
            Responses.Add(response);
        }

        private void Remove(Action<T1, T2, T3> response)
        {
            Responses.Remove(response);
        }
        
        public static GameEvent<T1, T2, T3> operator +(GameEvent<T1, T2, T3> gameEvent, Action<T1, T2, T3> response)
        {
            gameEvent.Register(response);
            return gameEvent;
        }

        public static GameEvent<T1, T2, T3> operator -(GameEvent<T1, T2, T3> gameEvent, Action<T1, T2, T3> response)
        {
            gameEvent.Remove(response);
            return gameEvent;
        }
    }
}