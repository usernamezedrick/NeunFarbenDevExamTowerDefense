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
            
        }

        public void OnExit()
        {
            
        }

        public void Update()
        {
            
        }
    }
}
