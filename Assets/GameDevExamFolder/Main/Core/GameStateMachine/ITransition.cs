namespace NF.Main.Core.GameStateMachine
{
    /// <summary>
    /// Represents a transition between two states.
    /// </summary>
    public interface ITransition
    {
        IState From { get; }
        IState To { get; }
        IPredicate Predicate { get; }
    }
}
