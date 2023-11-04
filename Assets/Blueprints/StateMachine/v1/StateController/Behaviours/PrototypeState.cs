using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Blueprints.StateController
{
    public class PrototypeState : MonoBehaviour, IState
    {
        public UnityEvent OnEnter;
        public UnityEvent OnIdle;
        public UnityEvent OnExit;

        [field: SerializeField] public float EnterTime { get; set; }
        [field: SerializeField] public float IdleTime { get; set; }
        [field: SerializeField] public float ExitTime { get; set; }

        public IEnumerator Enter()
        {
            OnEnter?.Invoke();
            yield return null;
        }

        public IEnumerator Idle()
        {
            OnIdle?.Invoke();
            yield return null;
        }

        public IEnumerator Exit()
        {
            OnExit?.Invoke();
            yield return null;
        }
    }
}

