using NF.Main.Gameplay.Managers;
using UnityEngine;

namespace NF.Main.Core.GameStateMachine
{
    public class GamePlayingState : GameBaseState
    {
        public GamePlayingState(GameManager gameManager, GameState state)
            : base(gameManager, state) { }

        public override void OnEnter() => Debug.Log("Entered Playing State");
        public override void OnExit() => Debug.Log("Exited Playing State");
        public override void Update()
        {
            // Add gameplay update logic here.
        }
    }
}
