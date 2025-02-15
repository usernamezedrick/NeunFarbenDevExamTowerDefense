using UnityEngine;

namespace NF.Main.Gameplay.Enemies
{
    public class EnemyMovingState : EnemyBaseState
    {
        private int _currentWaypointIndex;
        private readonly float _speed;
        private readonly Transform[] _waypoints;

        public EnemyMovingState(EnemyController enemy, Transform[] waypoints, float speed)
            : base(enemy)
        {
            _waypoints = waypoints;
            _speed = speed;
            _currentWaypointIndex = 0;
        }

        public override void OnEnter()
        {
            if (_waypoints.Length > 0)
            {
                _enemy.transform.position = _waypoints[0].position;
            }
        }

        public override void Update()
        {
            if (_currentWaypointIndex >= _waypoints.Length)
            {
                _enemy.DestroyEnemy();
                return;
            }

            MoveTowardsWaypoint();
        }

        private void MoveTowardsWaypoint()
        {
            Transform targetWaypoint = _waypoints[_currentWaypointIndex];
            _enemy.transform.position = Vector2.MoveTowards(
                _enemy.transform.position,
                targetWaypoint.position,
                _speed * Time.deltaTime
            );

            if (Vector2.Distance(_enemy.transform.position, targetWaypoint.position) < 0.1f)
            {
                _currentWaypointIndex++;
            }
        }

        public override void OnExit() { }
    }
}
