// using System;
// using System.Threading;
// using System.Threading.Tasks;
// using Blueprints.MainThreadDispatch;
// using Microsoft.AspNetCore.SignalR.Client;
// using UnityEngine;
//
// namespace Blueprints.Signal_R
// {
//     public class Hub : IHub
//     {
//         private readonly HubConnection _hubConnection;
//         private readonly MainThreadDispatcher _dispatcher;
//         private readonly CancellationTokenSource _source;
//         private readonly CancellationToken _token;
//         
//         private string _connectionId;
//         private HubConnectionState _state;
//
//         public Hub(MainThreadDispatcher dispatcher, string url)
//         {
//             _hubConnection = new HubConnectionBuilder()
//                 .WithUrl(url) // if access token required add options => { options.AccessTokenProvider = () => Task.FromResult(accessToken); })
//                 .WithAutomaticReconnect()
//                 .AddJsonProtocol()
//                 .Build();
//
//             _source = new CancellationTokenSource();
//             _token = _source.Token;
//             _dispatcher = dispatcher;
//
//             SetupActions();
//         }
//
//         string IHub.ConnectionId
//             => _hubConnection.ConnectionId;
//
//         HubConnectionState IHub.State
//             => _hubConnection.State;
//         
//         public void StartHubAsync(Action<bool> callback = null)
//             => ManageHubConnection(callback, HubConnectionState.Connected);
//
//         public void AddToGroupAsync(string groupId, Action<bool> callback = null)
//             => ControlHubGroups("AddToGroup", groupId, callback);
//         
//         public async void SendMessageToGroupAsync(string groupId, object message, Action<bool> callback = null)
//         {
//             if( _hubConnection.State == HubConnectionState.Connected )
//             {
//                 callback?.Invoke(false);
//             }
//             else
//             {
//                 await Task.Run(async () =>
//                 {
//                     try
//                     {
//                         await _hubConnection.InvokeAsync("SendMessageToGroup", groupId, message, _token);
//                         Dispatch(() => callback?.Invoke(true));
//                     }
//                     catch( Exception e )
//                     {
//                         Dispatch(() =>
//                         {
//                             Debug.LogError(e.Message);
//                             callback?.Invoke(false);
//                         });
//                     }
//                 }, _token);
//             }
//         }
//
//         public void RemoveFromGroupAsync(string groupId, Action<bool> callback = null)
//             => ControlHubGroups("RemoveFromGroup", groupId, callback);
//
//         public void StopHubAsync(Action<bool> callback = null)
//             => ManageHubConnection(callback, HubConnectionState.Disconnected);
//         
//         public void Dispose()
//         {
//             _source.Cancel();
//             Task.Run(() => _hubConnection.DisposeAsync());
//         }
//
//         private void Dispatch(Action action)
//             => _dispatcher.QueueAction(action);
//         
//         private void SetupActions()
//         {
//             _hubConnection.On<object>("MessageAsync", message =>
//             {
//                 Dispatch(() => IHub.ReceivedMessage?.Invoke(message));
//             });
//         }
//
//         private async void ManageHubConnection(Action<bool> callback, HubConnectionState stateOnCompletion)
//         {
//             if( _hubConnection.State == stateOnCompletion )
//             {
//                 callback?.Invoke(false);
//             }
//             else
//             {
//                 await Task.Run(async () =>
//                 {
//                     try
//                     {
//                         await _hubConnection.StopAsync(_token);
//                         Dispatch(() => callback?.Invoke(_hubConnection.State == stateOnCompletion));
//                     }
//                     catch( Exception e )
//                     {
//                         Dispatch(() => Debug.Log(e.Message));
//                     }
//                 }, _token);
//             }
//         }
//
//         private async void ControlHubGroups(string action, string groupId, Action<bool> callback)
//         {
//             await Task.Run(async () =>
//             {
//                 try
//                 {
//                     await _hubConnection.InvokeAsync(action, groupId, _token);
//                     Dispatch(() => callback?.Invoke(true));
//                 }
//                 catch( Exception e )
//                 {
//                     Dispatch(() =>
//                     {
//                         Debug.LogError(e.Message);
//                         callback?.Invoke(false);
//                     });
//                 }
//             }, _token);
//         }
//     }
// }