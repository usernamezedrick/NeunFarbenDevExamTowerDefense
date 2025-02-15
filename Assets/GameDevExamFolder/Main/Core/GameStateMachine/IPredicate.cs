namespace NF.Main.Core.GameStateMachine
{
    /// <summary>
    /// A predicate that returns true or false.
    /// </summary>
    public interface IPredicate
    {
        bool Evaluate();
    }
}
