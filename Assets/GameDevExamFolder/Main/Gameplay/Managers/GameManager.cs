using NF.Main.Core;
using NF.Main.Core.GameStateMachine;
using System;

namespace NF.Main.Gameplay.Managers
{
    public class GameManager : SingletonPersistent<GameManager>
    {
        // Use the GameState enum from our state machine namespace.
        public GameState CurrentGameState;

        private StateMachine _stateMachine;

        private void Awake()
        {
            // SingletonPersistent.Awake() automatically calls Initialize().
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        public override void Initialize(object data = null)
        {
            base.Initialize(data);
            CurrentGameState = GameState.Playing;
            SetupStateMachine();
        }

        private void SetupStateMachine()
        {
            _stateMachine = new StateMachine();

            // Create state instances. (Assuming GamePlayingState, GamePausedState, GameOverState exist similarly.)
            var playingState = new GamePlayingState(this, GameState.Playing);
            var pausedState = new GamePausedState(this, GameState.Paused);
            var gameOverState = new GameOverState(this, GameState.GameOver);
            var victoryState = new GameVictoryState(this, GameState.Victory);

            At(playingState, pausedState, new FuncPredicate(() => CurrentGameState == GameState.Paused));
            At(playingState, gameOverState, new FuncPredicate(() => CurrentGameState == GameState.GameOver));
            At(playingState, victoryState, new FuncPredicate(() => CurrentGameState == GameState.Victory));
            At(pausedState, playingState, new FuncPredicate(() => CurrentGameState == GameState.Playing));
            Any(playingState, new FuncPredicate(() => CurrentGameState == GameState.Playing));

            _stateMachine.SetState(playingState);
        }

        private void At(IState from, IState to, IPredicate condition) => _stateMachine.AddTransition(from, to, condition);
        private void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);
    }
}
