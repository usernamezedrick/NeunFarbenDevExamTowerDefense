using NF.Main.Gameplay.PlayerInput;
using UnityEngine;

namespace NF.Main.Core.PlayerStateMachine
{
    //Handles all logic for when player goes in, out, and during idle state
    public class PlayerIdleState: PlayerBaseState
    {
        public PlayerIdleState(PlayerController playerController, Animator animator) : base(playerController, animator)
        {
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            
            //Use this for transitioning between different animator hashes
            //_animator.CrossFade(IdleHash, 0.5f);
            
            Debug.Log("Entering Player Idle State");
        }

        public override void Update()
        {
            base.Update();
            Debug.Log("Player is Idling");
        }

        public override void OnExit()
        {
            base.OnExit();
            Debug.Log("Exiting Player Idle State");
        }
    }
}