using System;
using System.Collections.Generic;
using System.Linq;
using RequireInterface;
using UnityEngine;

namespace Blueprints.StateController.Core
{
    public abstract class MultiState<TState> : State<TState> where TState : Enum
    {
        private List<IState> _stateBehaviours;

        [SerializeField, RequireInterface(typeof(IState))]
        public List<UnityEngine.Object> stateBehaviours = new List<UnityEngine.Object>();

        public List<IState> Behaviours
        {
            get
            {
                if (_stateBehaviours.Any())
                {
                    return _stateBehaviours;
                }
                
                _stateBehaviours.Clear();
                
                foreach (var behaviour in stateBehaviours.Select(stateBehaviour => stateBehaviour as IState))
                {
                    if (behaviour == null)
                    {
                        throw new ArgumentNullException($"{name} State does not have a valid state behaviour set");
                    }
                    
                    _stateBehaviours.Add(behaviour);
                }

                return _stateBehaviours;
            }
        }
        
        protected virtual void OnEnable()
        {
            if (!stateBehaviours.Any())
            {
                throw new NullReferenceException($"State '{name}':  IState Behaviour unassigned");
            }
        }

        protected virtual void OnDisable()
        {
            if (!stateBehaviours.Any())
            {
                throw new NullReferenceException($"State '{name}':  IState Behaviour unassigned");
            }
        }
    }
}