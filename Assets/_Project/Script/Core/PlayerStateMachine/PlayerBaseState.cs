using NF.Main.Gameplay.PlayerInput;
using UnityEngine;

namespace NF.Main.Core.PlayerStateMachine
{
    public class PlayerBaseState: BaseState
    {
        protected readonly PlayerController _playerController;
        protected readonly Animator _animator;

        protected static readonly int IdleHash = Animator.StringToHash("Idle");
        protected static readonly int AttackHash = Animator.StringToHash("Attack");
        protected static readonly int HitHash = Animator.StringToHash("Hit");
        protected static readonly int DeathHash = Animator.StringToHash("Death");
        
        protected PlayerBaseState(PlayerController playerController, Animator animator)
        {
            _playerController = playerController;
            _animator = animator;
        }
    }
    
    public enum PlayerState
    {
        Idle,
        Attacking,
        Hit,
        Death
    }
}

