using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blueprints.StateController.Core;
using UnityEngine;

namespace Blueprints.StateMachine.StateController
{
    public abstract class StateController<TState> : StateMachine<TState> where TState : Enum
    {
        public Queue<State<TState>> Queue { get; } = new Queue<State<TState>>();
        
        [field: SerializeField] public int MaxQueueSize { get; private set; }

        private void OnDisable()
        {
            Queue.Clear();
        }

        public void ClearQueue() => Queue.Clear();

        public override void ChangeState(TState state)
        {
            if (!CanAddToQueue())
            {
                return;
            }

            AddToQueue(state);
            StartCoroutine(ChangeStateEnum(Queue.Dequeue()));
        }

        protected override IEnumerator ChangeStateEnum(State<TState> newState)
        {
            StateChanging = true;
            yield return CurrentState.Exit();
            CurrentState = newState;
            yield return CurrentState.Enter();
            yield return CurrentState.Idle();

            if (Queue.Any())
            {
                yield return ChangeStateEnum(Queue.Dequeue());
            }
            
            StateChanging = false;
        }

        protected void AddToQueue(TState state)
        {
            var newState = FindState(AvailableStates, state);
            Queue.Enqueue(newState);
        }

        protected bool CanAddToQueue()
        {
            return Queue.Count < MaxQueueSize;
        }
    }
}