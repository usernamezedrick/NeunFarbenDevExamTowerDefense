using UnityEngine;
using NF.Main.Core.GameStateMachine;

namespace NF.Main.Gameplay.Towers
{
    /// <summary>
    /// Tower Idle State: The tower waits for a target.
    /// </summary>
    public class TowerIdleState : IState
    {
        private TowerBase tower;

        public TowerIdleState(TowerBase tower)
        {
            this.tower = tower;
        }

        public void OnEnter()
        {
            // Optional: Add idle-specific logic here.
        }

        public void OnExit()
        {
            // Optional: Cleanup when exiting idle state.
        }

        public void Update()
        {
            // In idle state, no active action is required.
            // The state machine transition uses FindTarget() to trigger attack state.
        }
    }
}
