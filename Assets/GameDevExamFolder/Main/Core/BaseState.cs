using NF.Main.Core.GameStateMachine;
using UnityEngine;

namespace NF.Main.Core.StateMachine
{
    /// <summary>
    /// A base class for states that implements IState.
    /// </summary>
    public abstract class BaseState : IState
    {
        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void Update();
    }
}
