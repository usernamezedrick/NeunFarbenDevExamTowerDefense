namespace NF.Main.Core
{
    public interface ITransition
    {
        IState To { get; }
        IPredicate Condition { get; }
    }
}