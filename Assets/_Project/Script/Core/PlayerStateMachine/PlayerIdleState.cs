using NF.Main.Gameplay.PlayerInput;
using UnityEngine;

namespace NF.Main.Core.PlayerStateMachine
{
    /// <summary>
    /// A state representing the player idling.
    /// </summary>
    public class PlayerIdleState : PlayerBaseState
    {
        // Pass the PlayerController and specify PlayerState.Idle to the base constructor.
        public PlayerIdleState(PlayerController player) : base(player, PlayerState.Idle)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("Player has entered Idle State.");
            // Additional enter logic here.
        }

        public override void OnExit()
        {
            Debug.Log("Player is exiting Idle State.");
            // Additional exit logic here.
        }

        public override void Update()
        {
            // Idle state update logic (e.g., check for input to transition to another state).
        }
    }
}
