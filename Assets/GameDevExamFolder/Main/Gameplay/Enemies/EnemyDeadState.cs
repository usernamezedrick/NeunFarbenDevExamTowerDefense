using UnityEngine;

namespace NF.Main.Gameplay.Enemies
{
    public class EnemyDeadState : EnemyBaseState
    {
        public EnemyDeadState(EnemyController enemy) : base(enemy) { }

        public override void OnEnter()
        {
            _enemy.DestroyEnemy();
        }

        public override void Update() { }
        public override void OnExit() { }
    }
}
