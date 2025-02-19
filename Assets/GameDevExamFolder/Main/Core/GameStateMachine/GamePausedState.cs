using NF.Main.Gameplay.Managers;
using UnityEngine;

namespace NF.Main.Core.GameStateMachine
{
    public class GamePausedState : GameBaseState
    {
        public GamePausedState(GameManager gameManager, GameState state)
            : base(gameManager, state) { }

        public override void OnEnter() => Debug.Log("Entered Paused State");
        public override void OnExit() => Debug.Log("Exited Paused State");
        public override void Update()
        {

        }
    }
}
