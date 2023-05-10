using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Blueprints.Pooling.CustomPool
{
    public class ObjectPoolDemo : MonoBehaviour
    {
        [SerializeField] private ObjectPool pool;

        private IEnumerator Start()
        {
            while (true)
            {
                var obj = pool.GetObject();
                var position = Random.insideUnitSphere * 4;
                obj.transform.position = position;
                var delay = Random.Range(.1f, .5f);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}