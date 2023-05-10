using System.Collections;
using Blueprints.Utility;
using UnityEngine;

namespace Blueprints.Pooling.CustomPool
{
    public class ReturnAfterDelay : MonoBehaviour, IObjectPoolNotifier
    {
        public void OnEnqueuedToPool()
            => Debug.Log("Enqueued to pool");

        public void OnCreatedOrDequeuedFromPool(bool created)
            => this.ChainCoroutine(
                CoroutineUtils.WaitForSeconds(1),
                CoroutineUtils.Do(() =>
                {
                    gameObject.ReturnToPool();
                })
            );
    }
}