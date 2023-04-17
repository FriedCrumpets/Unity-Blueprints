using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Blueprints.Http
{
    public class HttpRequest : MonoBehaviour
    {
        public HttpRequestSettings_SO requestSettings;

        /// <summary>
        /// If true, then this request will repeat according to the value in <see cref="repeatInterval"/>
        /// </summary>
        public bool isRepeatingRequest;

        /// <summary>
        /// How many seconds between repeating requests when <see cref="isRepeatingRequest"/> is <c>true</c>
        /// </summary>
        [Min(10)]
        [Tooltip("Repeat interval in seconds")]
        public uint repeatInterval = 10;

        /// <summary>
        /// Control what lifecycle event triggers the HTTP request.
        /// </summary>
        public HttpRequestTrigger trigger;

        /// <summary>
        /// <see cref="UnityEvent{T0}"/> to trigger when HTTP response is returned.
        /// </summary>
        public UnityEvent<HttpResponse> onResponse;

        /// <summary>
        /// <see cref="UnityEvent{T0}"/> to trigger when HTTP response is processing.
        /// <typeparam name="{T0}"><c>bool</c> which will be true if request is loading.</typeparam>
        /// </summary>
        public UnityEvent<bool> onLoading;

        private float _repeatTimeRemaining;

        private bool _isRequestInFlight;

        private void Update()
        {
            if (isRepeatingRequest)
            {
                _repeatTimeRemaining -= Time.deltaTime;
                if (_repeatTimeRemaining <= 0)
                {
                    _repeatTimeRemaining = repeatInterval;
                    SendRequest();
                }
            }
        }

        private void Awake()
        {
            _repeatTimeRemaining = repeatInterval;
            Trigger(HttpRequestTrigger.Awake);
        }

        private void OnEnable() => Trigger(HttpRequestTrigger.OnEnable);

        private void Start() => Trigger(HttpRequestTrigger.Start);

        private void OnApplicationPause(bool pauseStatus) => Trigger(HttpRequestTrigger.OnApplicationPause);

        private void OnApplicationQuit() => Trigger(HttpRequestTrigger.OnApplicationQuit);

        private void OnDisable() => Trigger(HttpRequestTrigger.OnDisable);

        private void OnDestroy() => Trigger(HttpRequestTrigger.OnDestroy);

        private void Trigger(HttpRequestTrigger incomingTrigger)
        {
            if (trigger == incomingTrigger)
            {
                SendRequest();
            }
        }

        private void SendRequest()
        {
            if (_isRequestInFlight)
            {
                return;
            }

            _isRequestInFlight = true;
            onLoading.Invoke(true); 

            StartCoroutine(requestSettings.method switch
            {
                HttpMethod.Get => Get($"{requestSettings.baseUrl}{requestSettings.path}",
                    request => HandleResponse(new HttpResponse(request.downloadHandler.text))),
                _ => throw new NotImplementedException($"Http method {requestSettings.method} not implemented")
            });
        }

        private void HandleResponse(HttpResponse response)
        {
            onLoading.Invoke(false);
            onResponse.Invoke(response);
            _isRequestInFlight = false;
        }

        private static IEnumerator Get(string url, Action<UnityWebRequest> callback)
        {
            using var request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            callback(request);
        }
    }
}
