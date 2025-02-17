using NF.Main.Core;
using NF.Main.Core.GameStateMachine;
using UnityEngine;

namespace NF.Main.Gameplay.Managers
{
    public class GameManager : SingletonPersistent<GameManager>
    {
        public GameState CurrentGameState;
        private StateMachine _stateMachine;

        protected override void Awake()
        {
            base.Awake();

  
            if (transform.parent != null)
            {
                transform.SetParent(null);
            }

           
            DontDestroyOnLoad(gameObject);
            Time.timeScale = 1f;

            if (_stateMachine == null)
            {
                SetupStateMachine();
            }
        }

        private void Update()
        {
            _stateMachine?.Update();
        }

        public override void Initialize(object data = null)
        {
            base.Initialize(data);
            CurrentGameState = GameState.Paused; // Start game paused
            SetupStateMachine();
        }

        private void SetupStateMachine()
        {
            if (_stateMachine != null) return;

            _stateMachine = new StateMachine();

            var playingState = new GamePlayingState(this, GameState.Playing);
            var pausedState = new GamePausedState(this, GameState.Paused);
            var gameOverState = new GameOverState(this, GameState.GameOver);
            var victoryState = new GameVictoryState(this, GameState.Victory);

            _stateMachine.AddTransition(playingState, pausedState, new FuncPredicate(() => CurrentGameState == GameState.Paused));
            _stateMachine.AddTransition(pausedState, playingState, new FuncPredicate(() => CurrentGameState == GameState.Playing));
            _stateMachine.AddTransition(playingState, gameOverState, new FuncPredicate(() => CurrentGameState == GameState.GameOver));
            _stateMachine.AddTransition(playingState, victoryState, new FuncPredicate(() => CurrentGameState == GameState.Victory));

            _stateMachine.SetState(pausedState); // Set the initial state to paused
        }

        public void PauseGame()
        {
            if (_stateMachine == null) SetupStateMachine();
            if (CurrentGameState == GameState.GameOver) return;

            CurrentGameState = GameState.Paused;
            _stateMachine.SetState(new GamePausedState(this, GameState.Paused));
            Time.timeScale = 0f;
        }

        public void ResumeGame()
        {
            if (_stateMachine == null) SetupStateMachine();
            if (CurrentGameState == GameState.GameOver) return;

            CurrentGameState = GameState.Playing;
            _stateMachine.SetState(new GamePlayingState(this, GameState.Playing));
            Time.timeScale = 1f;
        }

        public void GameOver()
        {
            if (CurrentGameState == GameState.GameOver) return;

            Debug.Log("GameManager: Game Over triggered.");
            CurrentGameState = GameState.GameOver;
            _stateMachine.SetState(new GameOverState(this, GameState.GameOver));

         
            DestroyAllEnemies();

            Time.timeScale = 0f;
        }

        /// <summary>
        ///  Destroys all active enemies in the scene.
        /// </summary>
        private void DestroyAllEnemies()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
            Debug.Log("GameManager: All remaining enemies destroyed.");
        }

        public void Victory()
        {
            CurrentGameState = GameState.Victory;
            _stateMachine.SetState(new GameVictoryState(this, GameState.Victory));
        }

        public bool HasGameStarted()
        {
            return CurrentGameState == GameState.Playing;
        }

        public bool IsGamePaused()
        {
            return CurrentGameState == GameState.Paused;
        }
    }
}
