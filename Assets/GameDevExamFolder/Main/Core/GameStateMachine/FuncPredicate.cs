using System;

namespace NF.Main.Core.GameStateMachine
{
    /// <summary>
    /// A predicate that wraps a Func&lt;bool&gt;.
    /// </summary>
    public class FuncPredicate : IPredicate
    {
        private readonly Func<bool> _predicate;

        public FuncPredicate(Func<bool> predicate)
        {
            _predicate = predicate;
        }

        public bool Evaluate() => _predicate();
    }
}
