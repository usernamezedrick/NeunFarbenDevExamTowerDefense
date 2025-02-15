using NF.Main.Core.GameStateMachine;  // Needed for IState
using NF.Main.Gameplay.PlayerInput;      // Needed for PlayerController (adjust if PlayerController is in a different namespace)
using UnityEngine;

namespace NF.Main.Core.PlayerStateMachine
{
    public abstract class PlayerBaseState : IState
    {
        protected readonly PlayerController _player;
        protected readonly PlayerState _state;

        // Constructor that takes a PlayerController and a PlayerState value.
        public PlayerBaseState(PlayerController player, PlayerState state)
        {
            _player = player;
            _state = state;
        }

        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void Update();
    }
}
