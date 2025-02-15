namespace NF.Main.Core.GameStateMachine
{
    /// <summary>
    /// A concrete transition between states.
    /// </summary>
    public class Transition : ITransition
    {
        public IState From { get; }
        public IState To { get; }
        public IPredicate Predicate { get; }

        public Transition(IState from, IState to, IPredicate predicate)
        {
            From = from;
            To = to;
            Predicate = predicate;
        }
    }
}
