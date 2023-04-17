using System;
using System.Collections.Generic;
using UnityEngine;

namespace Blueprints.EventObserver
{
    public abstract class GameEvent<T> : ScriptableObject
    {
        protected readonly List<Action<T>> responses = new();

        public virtual void Raise(T args)
        {
            responses.ForEach(action => action.Invoke(args));
        }

        public void RegisterResponse(Action<T> response)
        {
            responses.Remove(response);
            responses.Add(response);
        }

        public void RemoveResponse(Action<T> response)
        {
            responses.Remove(response);
        }
    }
    
    public abstract class GameEvent<T1, T2> : ScriptableObject
    {
        protected readonly List<Action<T1, T2>> responses = new();

        public virtual void Raise(T1 arg1, T2 arg2)
        {
            responses.ForEach(action => action.Invoke(arg1, arg2));
        }

        public void RegisterResponse(Action<T1, T2> response)
        {
            responses.Remove(response);
            responses.Add(response);
        }

        public void RemoveResponse(Action<T1, T2> response)
        {
            responses.Remove(response);
        }
    }
    
    public abstract class GameEvent<T1, T2, T3> : ScriptableObject
    {
        protected readonly List<Action<T1, T2, T3>> responses = new();

        public virtual void Raise(T1 arg1, T2 arg2, T3 arg3)
        {
            responses.ForEach(action => action.Invoke(arg1, arg2, arg3));
        }

        public void RegisterResponse(Action<T1, T2, T3> response)
        {
            responses.Remove(response);
            responses.Add(response);
        }

        public void RemoveResponse(Action<T1, T2, T3> response)
        {
            responses.Remove(response);
        }
    }
}