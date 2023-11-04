using System.Collections;

namespace Blueprints.StateController
{
    public interface IState
    {
        /// <summary>
        /// The allocated time required for the state to complete Enter
        /// </summary>
        float EnterTime { get; set; }
        
        /// <summary>
        /// The allocated time required for the state to complete Idle
        /// </summary>
        float IdleTime { get; set; }
        
        /// <summary>
        /// The allocated time required for the state to complete Exit
        /// </summary>
        float ExitTime { get; set; }

        IEnumerator Enter();
        IEnumerator Idle();
        IEnumerator Exit();
    }
}