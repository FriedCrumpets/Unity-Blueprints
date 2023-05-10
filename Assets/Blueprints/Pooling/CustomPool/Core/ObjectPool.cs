using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;

namespace Blueprints.Pooling.CustomPool
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject poolObject;

        private Queue<GameObject> _inactiveObjects = new();

        public GameObject GetObject()
        {
            if (_inactiveObjects.Any())
            {
                var dequeuedObject = _inactiveObjects.Dequeue();
                
                // Sets parent to base of scene
                dequeuedObject.transform.parent = null;
                dequeuedObject.SetActive(true);
                
                // check for notifiers on the object and notify them that the object has left the pool 
                var notifiers = dequeuedObject.GetComponents<IObjectPoolNotifier>();
                foreach (var notifier in notifiers)
                    notifier.OnCreatedOrDequeuedFromPool(false);

                return dequeuedObject;
            }
            else
            {
                // there's nothing in the pool so create object and add to pool 
                var obj = Instantiate(poolObject);
                
                // Set up the pool tag on the object
                var poolTag = obj.AddComponent<PooledObject>();
                poolTag.Owner = this;
                poolTag.hideFlags = HideFlags.HideInInspector;

                // check for notifiers on the object and notify them that the object has left the pool 
                var notifiers = obj.GetComponents<IObjectPoolNotifier>();
                foreach (var notifier in notifiers)
                    notifier.OnCreatedOrDequeuedFromPool(true);

                return obj;
            }
        }

        public void ReturnObject(GameObject obj)
        {
            // check for notifiers on the object and notify them that the object has left the pool 
            var notifiers = obj.GetComponents<IObjectPoolNotifier>();
            foreach (var notifier in notifiers)
                notifier.OnEnqueuedToPool();
            
            obj.SetActive(false);
            obj.transform.parent = transform;
            
            _inactiveObjects.Enqueue(obj);
        }
    }

    public static class ObjectPoolExtensions
    {
        public static void ReturnToPool(this GameObject obj)
        {
            var pooledObject = obj.GetComponent<PooledObject>();
            if (pooledObject == null)
            {
                Debug.LogError($"Cannot return {obj.name} to a pool as it was not created from one");
                return;
            }
            
            pooledObject.Owner.ReturnObject(obj);
        }
    }
}