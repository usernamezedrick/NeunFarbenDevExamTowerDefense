using NF.Main.Gameplay;

namespace NF.Main.Core.GameStateMachine
{
    public class GameBaseState: BaseState
    {
        protected readonly GameManager _gameManager;
        protected readonly GameState _gameState;

        protected GameBaseState(GameManager gameManager, GameState gameState)
        {
            _gameManager = gameManager;
            _gameState = gameState;
        }
    }
}

public enum GameState
{
    Paused,
    Playing,
    GameOver
}