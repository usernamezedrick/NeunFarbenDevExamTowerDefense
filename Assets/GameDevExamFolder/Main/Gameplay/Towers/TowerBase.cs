using UnityEngine;
using NF.Main.Core.GameStateMachine; // Ensure IState, StateMachine, and FuncPredicate are defined

namespace NF.Main.Gameplay.Towers
{
    /// <summary>
    /// Generic base class for all towers.
    /// Contains properties like Range, FireRate, Damage, and methods such as Attack().
    /// Uses a state machine to toggle between idle and attack states.
    /// </summary>
    public abstract class TowerBase : MonoBehaviour
    {
        [Header("Tower Settings")]
        [SerializeField] protected float range = 5f;          // Detection range
        [SerializeField] protected float fireRate = 1f;         // Shots per second (Attack Speed)
        [SerializeField] protected int damage = 1;              // Damage dealt by the tower's bullets
        [SerializeField] protected GameObject bulletPrefab;     // Bullet prefab to instantiate
        [SerializeField] protected Transform firePoint;         // Spawn point for bullets

        [Header("Rotation Settings")]
        [SerializeField] protected float rotationSpeed = 90f;   // Degrees per second for smooth rotation

        [Header("Visual Settings")]
        [SerializeField] protected GameObject rangeIndicatorPrefab; // Optional range indicator

        [Header("Prediction Settings")]
        [SerializeField] protected bool enablePrediction = false;   // Enable predictive aiming
        [SerializeField] protected float bulletSpeedOverride = 10f;   // Used for prediction calculations

        protected Transform target;                              // Current enemy target (if any)
        public Transform CurrentTarget => target;                // Public accessor for the current target

        protected StateMachine stateMachine;                     // State machine to control tower behavior

        /// <summary>
        /// Exposes the fire rate for state machine calculations.
        /// </summary>
        public float FireRateValue => fireRate;

        /// <summary>
        /// Public getter for whether predictive aiming is enabled.
        /// </summary>
        public bool EnablePrediction => enablePrediction;

        /// <summary>
        /// Public getter for bullet speed used in prediction.
        /// </summary>
        public float BulletSpeedOverride => bulletSpeedOverride;

        /// <summary>
        /// Public getter for the fire point.
        /// </summary>
        public Transform FirePoint => firePoint;

        protected virtual void Start()
        {
            // Initialize the state machine with Idle and Attack states.
            stateMachine = new StateMachine();
            var idleState = new TowerIdleState(this);
            var attackState = new TowerAttackState(this);

            // Transition from idle to attack when a target is found.
            stateMachine.AddTransition(idleState, attackState, new FuncPredicate(() => FindTarget()));
            // Transition from attack to idle when the target is lost.
            stateMachine.AddTransition(attackState, idleState, new FuncPredicate(() => target == null));

            stateMachine.SetState(idleState);
        }

        protected virtual void Update()
        {
            // If the current target is destroyed or deactivated, clear it.
            if (target != null && !target.gameObject.activeInHierarchy)
            {
                target = null;
            }
            stateMachine.Update();
        }

        /// <summary>
        /// Searches for the nearest enemy within range.
        /// Sets the target if found and returns true; otherwise returns false.
        /// </summary>
        public virtual bool FindTarget()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float shortestDistance = Mathf.Infinity;
            Transform nearestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < shortestDistance && distance <= range)
                {
                    shortestDistance = distance;
                    nearestEnemy = enemy.transform;
                }
            }

            target = nearestEnemy;
            return target != null;
        }

        /// <summary>
        /// Smoothly rotates the tower to face its target (or predicted position if enabled).
        /// </summary>
        public virtual void RotateTowardsTarget()
        {
            if (target == null)
                return;

            Vector3 aimPoint = target.position;
            if (enablePrediction)
            {
                Rigidbody2D enemyRb = target.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    float distance = Vector2.Distance(firePoint.position, target.position);
                    float timeToHit = distance / bulletSpeedOverride;
                    aimPoint = target.position + (Vector3)enemyRb.linearVelocity * timeToHit;
                }
            }
            Vector2 direction = aimPoint - transform.position;
            float desiredAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            float currentAngle = transform.rotation.eulerAngles.z;
            float angle = Mathf.MoveTowardsAngle(currentAngle, desiredAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        /// <summary>
        /// Fires a bullet toward the current target.
        /// </summary>
        public virtual void Attack()
        {
            if (bulletPrefab == null || firePoint == null)
            {
                Debug.LogError("Bullet prefab or firePoint is not assigned on TowerBase!");
                return;
            }
            GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.damage = damage; // Set bullet damage from tower damage.
                bullet.SetTarget(target);
            }
            else
            {
                Debug.LogError("Bullet script is missing on the bullet prefab!");
            }
        }

        /// <summary>
        /// Toggles the range indicator when the tower is clicked.
        /// </summary>
        protected virtual void OnMouseDown()
        {
            ToggleRangeIndicator();
        }

        /// <summary>
        /// Instantiates or destroys the range indicator.
        /// The indicator is scaled uniformly to represent the tower's attack range.
        /// </summary>
        protected virtual void ToggleRangeIndicator()
        {
            if (rangeIndicatorPrefab == null)
                return;

            Transform indicatorTransform = transform.Find("RangeIndicator");
            if (indicatorTransform == null)
            {
                GameObject indicator = Instantiate(rangeIndicatorPrefab, transform.position, Quaternion.identity, transform);
                indicator.name = "RangeIndicator";
                // Use a uniform scale for a perfect circle.
                indicator.transform.localScale = Vector3.one * (range * 2);
            }
            else
            {
                Destroy(indicatorTransform.gameObject);
            }
        }
    }
}
