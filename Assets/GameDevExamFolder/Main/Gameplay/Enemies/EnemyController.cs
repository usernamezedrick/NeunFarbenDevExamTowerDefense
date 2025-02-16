using UnityEngine;
using UniRx;
using NF.Main.Core;

namespace NF.Main.Gameplay.Enemies
{
    public class EnemyController : MonoExt
    {
        [SerializeField] private Transform[] _waypoints;
        [SerializeField] private float _speed = 3f;
        [SerializeField] private int _health = 10;

        private int _currentWaypointIndex;
        private bool _isMoving = true;

        // Ensure LevelManager is in the correct namespace.
        private NF.Main.Gameplay.Managers.LevelManager _levelManager;

        // Exposed Animator property (make sure your enemy prefab has an Animator)
        public Animator Animator { get; private set; }

        // Event to notify when this enemy is destroyed.
        public Subject<Unit> OnEnemyDestroyed { get; private set; } = new Subject<Unit>();

        public override void Initialize()
        {
            base.Initialize();

            _levelManager = FindObjectOfType<NF.Main.Gameplay.Managers.LevelManager>();
            Animator = GetComponent<Animator>();

            if (_waypoints != null && _waypoints.Length > 0)
            {
                transform.position = _waypoints[0].position;
            }
        }

        public override void OnSubscriptionSet()
        {
            base.OnSubscriptionSet();
            AddEvent(OnEnemyDestroyed, _ => _levelManager?.EnemyDestroyed());
        }

        private void Update()
        {
            // If the game is paused, exit early.
            if (Time.timeScale == 0f)
                return;

            if (_isMoving && _waypoints != null && _waypoints.Length > 0)
            {
                MoveTowardsWaypoint();
            }
        }

        private void MoveTowardsWaypoint()
        {
            Transform targetWaypoint = _waypoints[_currentWaypointIndex];
            // Use Time.deltaTime to ensure movement stops when paused.
            transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, _speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                _currentWaypointIndex++;
                if (_currentWaypointIndex >= _waypoints.Length)
                {
                    DestroyEnemy();
                }
            }
        }

        public void DestroyEnemy()
        {
            OnEnemyDestroyed.OnNext(Unit.Default);
            Destroy(gameObject);
        }

        public void SetWaypoints(Transform[] newWaypoints)
        {
            _waypoints = newWaypoints;
        }

        public void SetSpeed(float newSpeed)
        {
            _speed = newSpeed;
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                DestroyEnemy();
            }
        }
    }
}
