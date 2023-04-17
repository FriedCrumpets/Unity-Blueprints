using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace Pooling
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 10f;
        [SerializeField] private float timeToSelfDestruct = 3f;
        
        public IObjectPool<Enemy> Pool { get; set; }
        public float CurrentHealth { get; set; }

        private void Start()
        {
            Reset();
        }

        private void OnEnable()
        {
            Attack();
            StartCoroutine(SelfDestruct());
        }

        private void OnDisable()
        {
            Reset();
        }

        public void TakeDamage(float amount)
        {
            CurrentHealth -= amount;

            if (CurrentHealth <= 0f)
            {
                ReturnToPool();
            }
        }

        private IEnumerator SelfDestruct()
        {
            yield return new WaitForSeconds(timeToSelfDestruct);

            TakeDamage(maxHealth);
        }

        private void ReturnToPool()
        {
            Pool.Release(this);
        }

        private void Reset()
        {
            CurrentHealth = maxHealth;
        }

        private void Attack()
        {
            Debug.Log("I ATtack");
        }
    }
}