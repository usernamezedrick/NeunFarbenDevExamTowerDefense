using UnityEngine;
using NF.Main.Core.GameStateMachine;

namespace NF.Main.Gameplay.Towers
{
    /// <summary>
    /// Attack state for the tower.
    /// The tower rotates toward its target (or predicted intercept point, if enabled) and fires only when aligned within a tolerance.
    /// </summary>
    public class TowerAttackState : IState
    {
        private TowerBase tower;
        private float attackCooldown;
        // Increased tolerance to 10 degrees for a less strict firing condition.
        private const float alignmentTolerance = 10f;

        public TowerAttackState(TowerBase tower)
        {
            this.tower = tower;
            attackCooldown = 0f;
        }

        public void OnEnter()
        {
            // Reset cooldown so the turret fires immediately upon entering attack state.
            attackCooldown = 0f;
        }

        public void OnExit()
        {
            // Optional cleanup when leaving the attack state.
        }

        public void Update()
        {
            // Update target continuously.
            tower.FindTarget();
            if (tower.CurrentTarget == null)
                return;

            // Rotate toward the target (or predicted intercept point if enabled).
            tower.RotateTowardsTarget();

            // Calculate the aim point.
            Vector3 aimPoint = tower.CurrentTarget.position;
            if (tower.EnablePrediction)
            {
                Rigidbody2D enemyRb = tower.CurrentTarget.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    float distance = Vector2.Distance(tower.FirePoint.position, tower.CurrentTarget.position);
                    float timeToHit = distance / tower.BulletSpeedOverride;
                    aimPoint = tower.CurrentTarget.position + (Vector3)enemyRb.linearVelocity * timeToHit;
                }
            }

            float desiredAngle = Mathf.Atan2((aimPoint - tower.transform.position).y, (aimPoint - tower.transform.position).x) * Mathf.Rad2Deg - 90f;
            float currentAngle = tower.transform.rotation.eulerAngles.z;
            float angleDiff = Mathf.Abs(Mathf.DeltaAngle(currentAngle, desiredAngle));

            // Only fire if the turret is sufficiently aligned.
            if (angleDiff > alignmentTolerance)
                return;

            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0f)
            {
                tower.Attack();
                attackCooldown = 1f / tower.FireRateValue;
            }
        }
    }
}
