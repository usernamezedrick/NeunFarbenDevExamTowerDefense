using UnityEngine;

namespace NF.Main.Gameplay.Enemies
{
    public abstract class EnemyBaseState : IEnemyState
    {
        protected readonly EnemyController _enemy;
      
        protected readonly Animator _animator;

        protected EnemyBaseState(EnemyController enemy)
        {
            _enemy = enemy;
            _animator = enemy.Animator;
        }

        public abstract void OnEnter();
        public abstract void Update();
        public virtual void FixedUpdate() { }
        public abstract void OnExit();
    }
}
