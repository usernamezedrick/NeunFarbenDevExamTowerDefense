using NF.Main.Gameplay.Managers;
using UnityEngine;

namespace NF.Main.Core.GameStateMachine
{
    /// <summary>
    /// A base class for game states that uses a GameManager.
    /// </summary>
    public abstract class GameBaseState : IState
    {
        protected readonly GameManager _gameManager;
        protected readonly GameState _state;

        public GameBaseState(GameManager gameManager, GameState state)
        {
            _gameManager = gameManager;
            _state = state;
        }

        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void Update();
    }
}
