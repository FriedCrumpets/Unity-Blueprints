using System;
using System.Collections.Generic;
using System.Linq;
using Blueprints.AsyncStateController.Core;
using Blueprints.AsyncStateController.Queue;
using UnityEngine;

namespace Blueprints.AsyncStateController.Async
{
    public abstract class AsyncStateMachine<TState> : QueueStateMachine<TState> where TState : Enum
    {
        private event Action<State<TState>> StateFinished;
        
        public IList<State<TState>> CurrentAsyncStates { get; private set; }

        /// <summary>
        /// The maximum number of states that can be executed asynchronously
        /// </summary>
        [field: SerializeField, Range(1,8)] public int MaxAsyncStates { get; private set; }

        private void OnEnable()
        {
            StateFinished += OnStateFinished;
        }

        private void OnDisable()
        {
            StateFinished -= OnStateFinished;
            CurrentAsyncStates.Clear();
            Queue.Clear();
        }

        public override void ChangeState(TState state)
        {
            if (StateCurrentlyExecuting(state))
            {
                return;
            }

            Action<TState> action = MaxAsyncStatesReached() switch
            {
                true => QueueState,
                false => ExecuteState,
            };

            action.Invoke(state);
        }

        protected override async void ChangeStateAsync(State<TState> state)
        {
            await state.Enter();
            await state.Idle();
            await state.Exit();
            StateFinished?.Invoke(state);
        }
        
        private void QueueState(TState state)
        {
            if (CanAddToQueue())
            {
                AddToQueue(state);
            }
        }

        private void ExecuteState(TState state)
        {
            var newState = GetNewState(AvailableStates, state);
            CurrentAsyncStates.Add(newState);
            ChangeStateAsync(newState);
        }

        private void OnStateFinished(State<TState> state)
        {
            CurrentAsyncStates.Remove(state);

            if (MaxAsyncStatesReached())
            {
                return;
            }

            if (!Queue.Any())
            {
                return;
            }
            
            var newState = Queue.Dequeue();
                
            if (StateCurrentlyExecuting(newState.CommandingState))
            {
                return;
            }
                
            CurrentAsyncStates.Add(newState);
            ChangeStateAsync(newState);
        }

        private bool MaxAsyncStatesReached() => CurrentAsyncStates.Count >= MaxAsyncStates;

        private bool StateCurrentlyExecuting(TState state)
        {
            var currentStates = CurrentAsyncStates.Select(item => item.CommandingState);
            return currentStates.Contains(state);
        }
    }
}