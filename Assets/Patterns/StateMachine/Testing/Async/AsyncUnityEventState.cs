using System;
using System.Threading.Tasks;
using Patterns.StateMachine.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Patterns.StateMachine.Async
{
    public class AsyncUnityEventState : MonoBehaviour, IStateBehaviourAsync
    {
        [field: SerializeField] [field: Tooltip("The allocated time required for the state to complete Enter")]
        public float EnterTime { get; set; }
        
        [field: SerializeField] [field: Tooltip("The allocated time required for the state to complete Idle")]
        public float IdleTime { get; set; }
        
        [field: SerializeField] [field: Tooltip("The allocated time required for the state to complete Exit")]
        public float ExitTime { get; set; }

        public UnityEvent OnEnter;
        public UnityEvent OnIdle;
        public UnityEvent OnExit;
        
        public async Task Enter()
        {
            await Task.Run(OnEnter.Invoke);
        }

        public async Task Idle()
        {
            await Task.Run(OnIdle.Invoke);
        }

        public async Task Exit()
        {
            await Task.Run(OnExit.Invoke);
        }
    }
}