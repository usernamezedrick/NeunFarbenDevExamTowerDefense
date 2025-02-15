using NF.Main.Core;
using NF.Main.Core.GameStateMachine;

namespace NF.Main.Gameplay
{
    public class GameManager : SingletonPersistent<GameManager>
    {
        public GameState GameState;

        private StateMachine _stateMachine;
        
        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        public override void Initialize(object data = null)
        {
            base.Initialize(data);
            GameState = GameState.Playing;
            SetupStateMachine();
        }

        private void SetupStateMachine()
        {
            // State Machine
            _stateMachine = new StateMachine();

            // Declare states
            var pausedState = new GamePausedState(this, GameState.Paused);
            var playingState = new GamePlayingState(this, GameState.Playing);
            var gameOverState = new GameOverState(this, GameState.GameOver);


            // Define transitions
            At(playingState, pausedState, new FuncPredicate(() => GameState == GameState.Paused));
            At(playingState, gameOverState, new FuncPredicate(() => GameState == GameState.GameOver));
            
            Any(playingState, new FuncPredicate(() => GameState == GameState.Playing));

            // Set initial state
            _stateMachine.SetState(playingState);
        }

        private void At(IState from, IState to, IPredicate condition) => _stateMachine.AddTransition(from, to, condition);
        private void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);
    }
}