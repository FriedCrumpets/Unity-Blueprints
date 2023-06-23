using System;
using System.Collections.Generic;
using UnityEngine;

namespace Logging
{
    public interface ICheckNetworkStability
    {
        double Ping { get; }

        void State();
        Type NetType();
    }
    
    public class NetworkStabilityLogger : Debugger, ICheckNetworkStability
    {
        public NetworkStabilityLogger(string name, ILogHandler logHandler, ICheckNetworkStability wrap) : base(name, logHandler)
        {
            wrapped = wrap;
        }
        
        [SerializeReference] private ICheckNetworkStability wrapped;

        public double Ping
        {
            get
            {
                Log("Ping retrieved", this);
                return wrapped.Ping;
            }
        }
        
        public void State()
        {
            wrapped.State();
            Log("State retrieved", this);
        }

        public Type NetType()
        {
            Log("NetType retrieved", this); 
            return wrapped.NetType();
        }
    }

    public static class NetworkStabilityLocator
    {
        public static ICheckNetworkStability Service { get; private set; }

        public static void Provide(ICheckNetworkStability service)
        {
            Service = service;
        }
    } 
    
public class Solution 
{
    //s = "([}}])"
    public bool IsValid(string s) 
    {
        if(s.Length <= 1) return false;
        if(s.Length % 2 == 1) return false;

        var stack = new Stack<char>();

        foreach (var c in s)
        {
            switch (c)
            {
                case '(':
                    stack.Push(')');
                    break;  
                case '[':
                    stack.Push(']');
                    break;
                case '{':
                    stack.Push('}');
                    break;
            }

            if (stack.Count == 0)
                continue;

            switch (c)
            {
                case ')' when stack.Peek() != ')':
                case '}' when stack.Peek() != '}':
                case ']' when stack.Peek() != ']':
                    return false;
            }

            if (c.Equals(stack.Peek()))
                stack.Pop();
        }

        return stack.Count == 0;
    }
}
}