using Blueprints.StateMachine.Mono;
using UnityEngine;
using UnityEngine.Events;

namespace Blueprints.StateMachine
{
    public class UnityEventStateBehaviourController : MonoBehaviour, IStateBehaviour
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
        
        public void Enter()
        {
            OnEnter?.Invoke();
        }

        public void Idle()
        {
            OnIdle?.Invoke();
        }

        public void Exit()
        {
            OnExit?.Invoke();
        }
    }
}