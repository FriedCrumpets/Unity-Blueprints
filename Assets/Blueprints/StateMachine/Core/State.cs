using System.Threading.Tasks;
using UnityEngine;

namespace Blueprints.StateMachine.Core
{
    public abstract class State<TState> : MonoBehaviour
    {
        public TState CommandingState { get; set; }
        
        public abstract Task Enter();
        public abstract Task Idle();
        public abstract Task Exit();
    }
}