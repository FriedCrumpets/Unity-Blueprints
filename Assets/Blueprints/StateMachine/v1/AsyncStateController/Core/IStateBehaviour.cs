using System.Threading.Tasks;

namespace Blueprints.AsyncStateController.Core
{
    public interface IStateBehaviour
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

        Task Enter();
        Task Idle();
        Task Exit();
    }
}