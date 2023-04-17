using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Blueprints.StateController
{
    public abstract class StateController<TState> : StateMachine<TState> where TState : Enum
    {
        public Queue<State<TState>> Queue { get; } = new();

        [field: SerializeField] public int MaxQueueSize { get; private set; }

        private void OnDisable() => Queue.Clear();

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

        public void Next()
        {
            var currentIndex = States.FindIndex(state => state == CurrentState);

            if (currentIndex + 1 < States.Count)
            {
                ChangeState(States[currentIndex+1].CommandingState);
            }
        }

        public void Previous()
        {
            var currentIndex = States.FindIndex(state => state == CurrentState);

            if (currentIndex - 1 > -1)
            {
                ChangeState(States[currentIndex-1].CommandingState);
            }
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

        protected bool CanAddToQueue() => Queue.Count < MaxQueueSize;
    }
}