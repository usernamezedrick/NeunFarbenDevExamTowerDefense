using NF.Main.Gameplay.Managers;
using UnityEngine;

namespace NF.Main.Core.GameStateMachine
{
    public class GamePlayingState : IState
    {
        private readonly GameManager _gameManager;
        private readonly GameState _state;
        private bool _hasLoggedExit;

        public GamePlayingState(GameManager gameManager, GameState state)
        {
            _gameManager = gameManager;
            _state = state;
            _hasLoggedExit = false;
        }

        public void OnEnter()
        {
#if UNITY_EDITOR
            Debug.Log("Entered Playing State");
#endif
            _hasLoggedExit = false;
        }

        public void OnExit()
        {
            if (!_hasLoggedExit)
            {
#if UNITY_EDITOR
                Debug.Log("Exited Playing State");
#endif
                _hasLoggedExit = true;
            }
        }

        public void Update()
        {
           
        }
    }
}
