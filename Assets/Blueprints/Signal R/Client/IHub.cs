// using System;
// using Microsoft.AspNetCore.SignalR.Client;
//
// namespace Blueprints.Signal_R
// {
//     public interface IHub : IDisposable
//     {
//         /*
//          * Each Hub built ontop of this would have static actions that are subscribed to and invoked
//          * by users of the hub interface.
//          *
//          * Example
//          *
//          * static Action<object> ReceivedMessage;
//          *
//          * on implementation of interface
//          *
//          * private void SetupActions()
//          * {
//          *      _hubConnection.On<object>("MessageAsync", message => _bus.Publish<Action>(
//          *          new Action(() => IHub.ReceivedMessage?.Invoke(message)))
//          *      );
//          * }
//          */
//
//         static Action<object> ReceivedMessage;
//         
//         internal string ConnectionId { get; }
//         internal HubConnectionState  State { get; }
//
//         void StartHubAsync(Action<bool> callback = null);
//         void AddToGroupAsync(string groupId, Action<bool> callback = null);
//         void RemoveFromGroupAsync(string groupId, Action<bool> callback = null);
//         void SendMessageToGroupAsync(string groupId, object message, Action<bool> callback = null);
//         void StopHubAsync(Action<bool> callback = null);
//     }
// }