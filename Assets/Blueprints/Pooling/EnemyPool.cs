using UnityEngine;

namespace Pooling
{
    public class EnemyPool : ObjectPool<Enemy>
    {
        public EnemyPool(int maxPoolSize, int defaultCapacity) : base(maxPoolSize, defaultCapacity) { }
        
        public override void Spawn()
        {
            var enemy = Pool.Get();

            var randomx = Random.Range(0, 3);
            var randomy = Random.Range(0, 3);
            var randomz = Random.Range(0, 3);

            enemy.transform.position = Vector3.zero + new Vector3(randomx, randomy, randomz);
        }

        protected override Enemy CreateItem()
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var enemy = go.AddComponent<Enemy>();

            go.name = "Enemy";
            enemy.Pool = Pool;

            return enemy;
        }

        protected override void OnRetrieveFromPool(Enemy enemy)
        {
            enemy.gameObject.SetActive(true);
        }

        protected override  void OnReturnToPool(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
        }

        protected override void OnDestroyPoolObject(Enemy enemy)
        {
            Object.Destroy(enemy.gameObject);
        }
    }
}