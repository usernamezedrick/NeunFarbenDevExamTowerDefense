using NF.Main.Gameplay;
using UnityEngine;

namespace NF.Main.Core.GameStateMachine
{
    public class GameOverState : GameBaseState
    {
        public GameOverState(GameManager gameManager, GameState gameState) : base(gameManager, gameState)
        {
        
        }
    
        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Game over state");
        }
    }
}