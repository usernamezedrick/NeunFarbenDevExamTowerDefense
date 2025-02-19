using NF.Main.Core.GameStateMachine;
using UnityEngine;

namespace NF.Main.Gameplay.Managers
{
    public class GameVictoryState : IState
    {
        private readonly GameManager _gameManager;
        private readonly GameState _state;

        public GameVictoryState(GameManager gameManager, GameState state)
        {
            _gameManager = gameManager;
            _state = state;
        }

        public void OnEnter()
        {
            Debug.Log("Entered Victory State");
            
        }

        public void OnExit()
        {
            Debug.Log("Exited Victory State");
        }

        public void Update()
        {
           
        }
    }
}
