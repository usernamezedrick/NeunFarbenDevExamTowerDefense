using UnityEngine;
using NF.Main.Core.GameStateMachine;

namespace NF.Main.Gameplay.Towers
{
    /// <summary>
    /// Attack state for the tower.
    /// </summary>
    public class TowerAttackState : IState
    {
        private TowerBase tower;
        private float attackCooldown;
        
        private const float alignmentTolerance = 10f;

        public TowerAttackState(TowerBase tower)
        {
            this.tower = tower;
            attackCooldown = 0f;
        }

        public void OnEnter()
        {
            
            attackCooldown = 0f;
        }

        public void OnExit()
        {
           
        }

        public void Update()
        {
          
            tower.FindTarget();
            if (tower.CurrentTarget == null)
                return;

            tower.RotateTowardsTarget();

         
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
