namespace Blueprints.Pooling.CustomPool
{
    public interface IObjectPoolNotifier
    {
        /// <summary>
        /// Called when the object is returned to the pool
        /// </summary>
        void OnEnqueuedToPool();

        /// <summary>
        /// Called when object is leaving the pool/has just been created
        /// </summary>
        /// <param name="created"></param>
        void OnCreatedOrDequeuedFromPool(bool created);
    }
}