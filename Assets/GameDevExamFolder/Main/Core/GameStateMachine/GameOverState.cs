using NF.Main.Gameplay.Managers;
using UnityEngine;

namespace NF.Main.Core.GameStateMachine
{
    public class GameOverState : GameBaseState
    {
        public GameOverState(GameManager gameManager, GameState state)
            : base(gameManager, state) { }

        public override void OnEnter() => Debug.Log("Entered GameOver State");
        public override void OnExit() => Debug.Log("Exited GameOver State");
        public override void Update()
        {
           
        }
    }
}
