using NF.Main.Core;
using NF.Main.Core.GameStateMachine;
using NF.Main.Gameplay.Enemies;
using NF.Main.Gameplay.Towers;
using UnityEngine;
using UnityEngine.SceneManagement; // NEW: For scene loaded events

namespace NF.Main.Gameplay.Managers
{
    public class GameManager : SingletonPersistent<GameManager>
    {
        public GameState CurrentGameState;
        private StateMachine _stateMachine;

        // ===== NEW: Currency System =====
        public int currency = 10; // Initial currency set to $10

        public void AddCurrency(int amount)
        {
            currency += amount;
            Debug.Log("Currency Gained: " + amount + " | Total: " + currency);
        }

        public bool CanAfford(int cost)
        {
            return currency >= cost;
        }

        public void SpendCurrency(int amount)
        {
            if (CanAfford(amount))
            {
                currency -= amount;
                Debug.Log("Currency Spent: " + amount + " | Remaining: " + currency);
            }
        }
        // ===== End Currency System =====

        protected override void Awake()
        {
            base.Awake();

            if (transform.parent != null)
                transform.SetParent(null);

            DontDestroyOnLoad(gameObject);
            Time.timeScale = 1f;

            // Subscribe to scene loaded events so we can reset currency on restart
            SceneManager.sceneLoaded += OnSceneLoaded;

            if (_stateMachine == null)
                SetupStateMachine();
        }

        private void OnDestroy()
        {
            // Unsubscribe from scene loaded events
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Reset currency on new scene load (i.e., when restarting the game)
            ResetGame();
        }

        public override void Initialize(object data = null)
        {
            base.Initialize(data);
            CurrentGameState = GameState.Paused;
            SetupStateMachine();
            // (You can optionally call ResetGame() here as well)
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

            _stateMachine.SetState(pausedState);
        }

        // NEW: ResetGame method to reset currency and any other variables for a new game
        public void ResetGame()
        {
            currency = 10;
            Debug.Log("Game reset: Currency is now $" + currency);
            // Reset any additional game state here if needed.
        }

        public void PauseGame()
        {
            if (_stateMachine == null) SetupStateMachine();
            if (CurrentGameState == GameState.GameOver || CurrentGameState == GameState.Victory) return;

            CurrentGameState = GameState.Paused;
            _stateMachine.SetState(new GamePausedState(this, GameState.Paused));
            Time.timeScale = 0f;
        }

        public void ResumeGame()
        {
            if (_stateMachine == null) SetupStateMachine();
            if (CurrentGameState == GameState.GameOver || CurrentGameState == GameState.Victory) return;

            CurrentGameState = GameState.Playing;
            _stateMachine.SetState(new GamePlayingState(this, GameState.Playing));
            Time.timeScale = 1f;
        }

        public void GameOver()
        {
            if (CurrentGameState == GameState.GameOver) return;

            CurrentGameState = GameState.GameOver;
            _stateMachine.SetState(new GameOverState(this, GameState.GameOver));

            DestroyAllTurrets();
            DestroyAllEnemies();

            Time.timeScale = 0f;
        }

        public void Victory()
        {
            if (CurrentGameState == GameState.Victory) return;

            CurrentGameState = GameState.Victory;
            _stateMachine.SetState(new GameVictoryState(this, GameState.Victory));

            DestroyAllTurrets();
            DestroyAllEnemies();

            UIManager.Instance.ShowVictoryScreen();
            Time.timeScale = 0f;
        }

        private void DestroyAllTurrets()
        {
            TowerBase[] turrets = FindObjectsOfType<TowerBase>();
            foreach (TowerBase turret in turrets)
            {
                Destroy(turret.gameObject);
            }
        }

        private void DestroyAllEnemies()
        {
            EnemyController[] enemies = FindObjectsOfType<EnemyController>();
            foreach (EnemyController enemy in enemies)
            {
                Destroy(enemy.gameObject);
            }
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
