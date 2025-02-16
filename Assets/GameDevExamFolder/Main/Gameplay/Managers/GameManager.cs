using NF.Main.Core;
using NF.Main.Core.GameStateMachine;
using System;
using UnityEngine;

namespace NF.Main.Gameplay.Managers
{
    public class GameManager : SingletonPersistent<GameManager>
    {
        // Use the GameState enum from our state machine namespace.
        public GameState CurrentGameState;

        private StateMachine _stateMachine;

        protected override void Awake()
        {
            base.Awake();
            // Optionally, force a starting time scale.
            // For example, if you want the game to start unpaused by default:
            // Time.timeScale = 1f;
        }

        private void Update()
        {
            if (_stateMachine != null)
                _stateMachine.Update();
        }

        public override void Initialize(object data = null)
        {
            base.Initialize(data);
            // Set the initial game state. (You can choose Playing or Paused as your default.)
            // For this example, we assume the game starts unpaused.
            CurrentGameState = GameState.Playing;
            SetupStateMachine();
        }

        private void SetupStateMachine()
        {
            _stateMachine = new StateMachine();

            // Create state instances (make sure these state classes exist).
            var playingState = new GamePlayingState(this, GameState.Playing);
            var pausedState = new GamePausedState(this, GameState.Paused);
            var gameOverState = new GameOverState(this, GameState.GameOver);
            var victoryState = new GameVictoryState(this, GameState.Victory);

            // Define transitions.
            At(playingState, pausedState, new FuncPredicate(() => CurrentGameState == GameState.Paused));
            At(playingState, gameOverState, new FuncPredicate(() => CurrentGameState == GameState.GameOver));
            At(playingState, victoryState, new FuncPredicate(() => CurrentGameState == GameState.Victory));
            At(pausedState, playingState, new FuncPredicate(() => CurrentGameState == GameState.Playing));
            Any(playingState, new FuncPredicate(() => CurrentGameState == GameState.Playing));

            // Set initial state.
            if (CurrentGameState == GameState.Playing)
                _stateMachine.SetState(playingState);
            else
                _stateMachine.SetState(pausedState);
        }

        private void At(IState from, IState to, IPredicate condition) =>
            _stateMachine.AddTransition(from, to, condition);

        private void Any(IState to, IPredicate condition) =>
            _stateMachine.AddAnyTransition(to, condition);

        /// <summary>
        /// Pauses the game by setting the state to Paused and setting Time.timeScale to 0.
        /// </summary>
        public void PauseGame()
        {
            CurrentGameState = GameState.Paused;
            // Transition to a new instance of the paused state.
            _stateMachine.SetState(new GamePausedState(this, GameState.Paused));
            Time.timeScale = 0f;
            Debug.Log("GameManager: Game paused.");
        }

        /// <summary>
        /// Resumes the game by setting the state to Playing and setting Time.timeScale to 1.
        /// </summary>
        public void ResumeGame()
        {
            CurrentGameState = GameState.Playing;
            // Transition to a new instance of the playing state.
            _stateMachine.SetState(new GamePlayingState(this, GameState.Playing));
            Time.timeScale = 1f;
            Debug.Log("GameManager: Game resumed.");
        }
    }
}
