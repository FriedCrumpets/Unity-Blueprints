using System;
using System.Collections.Generic;
using System.Linq;
using Blueprints.Attributes;
using UnityEngine;

namespace Blueprints.AsyncStateController.Core
{
    public abstract class MultiState<TState> : State<TState> where TState : Enum
    {
        private List<IStateBehaviour> _stateBehaviours;

        [SerializeField, RequireInterface(typeof(IStateBehaviour))]
        public List<UnityEngine.Object> stateBehaviours = new List<UnityEngine.Object>();

        public List<IStateBehaviour> StateBehaviours
        {
            get
            {
                if (_stateBehaviours.Any())
                {
                    return _stateBehaviours;
                }
                
                _stateBehaviours.Clear();
                
                foreach (var behaviour in stateBehaviours.Select(stateBehaviour => stateBehaviour as IStateBehaviour))
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
                throw new StateException($"State '{name}':  IState Behaviour unassigned");
            }
        }

        protected virtual void OnDisable()
        {
            if (!stateBehaviours.Any())
            {
                throw new StateException($"State '{name}':  IState Behaviour unassigned");
            }
        }
    }
}