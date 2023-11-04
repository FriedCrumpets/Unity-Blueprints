using System;
using System.Collections.Generic;
using System.Linq;
using Blueprints.AsyncStateController.Core;
using UnityEngine;

namespace Blueprints.AsyncStateController.Queue
{
    public abstract class QueueStateMachine<TState> : StateMachine<TState> where TState : Enum
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
            ChangeStateAsync(Queue.Dequeue());
        }

        protected override async void ChangeStateAsync(State<TState> newState)
        {
            StateChanging = true;
            await CurrentState.Exit();
            CurrentState = newState;
            await CurrentState.Enter();
            await CurrentState.Idle();

            if (Queue.Any())
            {
                ChangeStateAsync(Queue.Dequeue());
            }
            
            StateChanging = false;
        }

        protected void AddToQueue(TState state)
        {
            var newState = GetNewState(AvailableStates, state);
            Queue.Enqueue(newState);
        }

        protected bool CanAddToQueue()
        {
            return Queue.Count < MaxQueueSize;
        }
    }
}