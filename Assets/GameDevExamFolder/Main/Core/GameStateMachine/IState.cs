namespace NF.Main.Core.GameStateMachine
{
    /// <summary>
    /// A simple interface for states.
    /// </summary>
    public interface IState
    {
        void OnEnter();
        void OnExit();
        void Update();
    }
}
