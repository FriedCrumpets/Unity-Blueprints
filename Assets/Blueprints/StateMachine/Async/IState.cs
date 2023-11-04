using System.Threading.Tasks;

namespace Blueprints.StateMachine.Async
{
    public interface IState
    {
        Task Enter();
        Task Update();
        Task Physics();
        Task Exit();
    }
}