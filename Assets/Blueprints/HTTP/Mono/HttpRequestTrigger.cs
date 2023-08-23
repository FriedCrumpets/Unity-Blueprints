namespace Blueprints.Http
{
    public enum HttpRequestTrigger
    {
        /// <summary>
        /// Never trigger the HTTP Request.
        /// Useful when triggering the request via code.
        /// </summary>
        None,

        /// <summary>
        /// Trigger the HTTP request on Awake.
        /// </summary>
        Awake,

        /// <summary>
        /// Trigger the HTTP request on OnEnable.
        /// </summary>
        OnEnable,

        /// <summary>
        /// Trigger the HTTP request on Start.
        /// </summary>
        Start,

        /// <summary>
        /// Trigger the HTTP request on OnApplicationPause.
        /// </summary>
        OnApplicationPause,

        /// <summary>
        /// Trigger the HTTP request on OnApplicationQuit.
        /// </summary>
        OnApplicationQuit,

        /// <summary>
        /// Trigger the HTTP request on OnDisable.
        /// </summary>
        OnDisable,

        /// <summary>
        /// Trigger the HTTP request on OnDestroy.
        /// </summary>
        OnDestroy,
    }
}
