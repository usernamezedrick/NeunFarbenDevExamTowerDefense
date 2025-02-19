using NF.Main.Gameplay.Enemies;
using UnityEngine;

namespace NF.Main.Gameplay.Towers
{
    /// <summary>
    /// Handles bullet behavior: moving toward its target, rotating, and applying damage upon impact.
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        public float speed = 10f;   
        public int damage = 1;      

        private Transform target;   
        private Rigidbody2D rb;    

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Sets the bullet's target.
        /// </summary>
        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        private void Update()
        {
            if (target == null)
            {
                Destroy(gameObject); 
                return;
            }

            
            Vector2 direction = (target.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
            RotateBullet(direction);
        }

        /// <summary>
        /// Rotates the bullet to face its movement direction.
        /// </summary>
        private void RotateBullet(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                var enemy = collision.GetComponent<EnemyController>();
                if (enemy != null)
                {
                  
                    enemy.TakeDamage(damage);
                }
                else
                {
                  
                    Destroy(collision.gameObject);
                }
               
                Destroy(gameObject);
            }
        }
    }
}
