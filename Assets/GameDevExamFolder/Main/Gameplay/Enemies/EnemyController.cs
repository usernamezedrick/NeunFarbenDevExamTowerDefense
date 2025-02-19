using UnityEngine;
using UniRx;
using NF.Main.Core;
using NF.Main.Gameplay.Managers;

namespace NF.Main.Gameplay.Enemies
{
    public class EnemyController : MonoExt
    {
        [SerializeField] private Transform[] _waypoints; 
        [SerializeField] private float _speed = 3f;
        [SerializeField] private int _health = 10;

        private int _currentWaypointIndex;
        private bool _isMoving = true;

        
        public Animator Animator { get; private set; }

        
        public Subject<Unit> OnEnemyDestroyed { get; private set; } = new Subject<Unit>();

        
        private CompositeDisposable disposables = new CompositeDisposable();

        public override void Initialize()
        {
            base.Initialize();
            Animator = GetComponent<Animator>();

            
            if (_waypoints != null && _waypoints.Length > 0)
            {
                transform.position = _waypoints[0].position;
            }

            
            OnEnemyDestroyed
                .Subscribe(_ => Debug.Log("Enemy Destroyed"))
                .AddTo(disposables);
        }

        private void Update()
        {
            
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
            transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, _speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                _currentWaypointIndex++;
                if (_currentWaypointIndex >= _waypoints.Length)
                {
                    
                    var playerHealth = FindObjectOfType<PlayerHealthManager>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage();
                    }
                    else
                    {
                        Debug.LogWarning("EnemyController: PlayerHealthManager not found!");
                    }
                    DestroyEnemy();
                }
            }
        }

        public void DestroyEnemy()
        {
            
            GameManager.Instance.AddCurrency(5);
            OnEnemyDestroyed.OnNext(Unit.Default);
            OnEnemyDestroyed.OnCompleted();
            Destroy(gameObject);
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                DestroyEnemy();
            }
        }

        /// <summary>
        /// Sets the waypoints for this enemy.
        /// </summary>
        /// <param name="newWaypoints">An array of Transforms representing the path.</param>
        public void SetWaypoints(Transform[] newWaypoints)
        {
            _waypoints = newWaypoints;
            _currentWaypointIndex = 0;

            if (_waypoints != null && _waypoints.Length > 0)
            {
                transform.position = _waypoints[0].position;
            }
        }
    }
}
