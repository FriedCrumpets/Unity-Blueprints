using System;
using System.Threading.Tasks;

namespace Blueprints.StateMachine.AsyncStates.Core
{
    public interface IStateAsync
    {
        Task Enter(Action action = null);
        Task Idle(Action action = null);
        Task Exit(Action action = null);
    }
    
    public static class StateAsyncExtensions
    {
        public static async void Execute(this IStateAsync state)
        {
            await state.Enter();
            await state.Idle();
            await state.Exit();
        }
    }
}