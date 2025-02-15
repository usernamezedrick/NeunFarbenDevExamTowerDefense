using System.Collections.Generic;

namespace NF.Main.Core.GameStateMachine
{
    /// <summary>
    /// A basic state machine that handles transitions and state updates.
    /// </summary>
    public class StateMachine
    {
        private IState _currentState;
        private readonly List<Transition> _transitions = new List<Transition>();
        private readonly List<Transition> _anyTransitions = new List<Transition>();

        public void SetState(IState newState)
        {
            _currentState?.OnExit();
            _currentState = newState;
            _currentState.OnEnter();
        }

        public void AddTransition(IState from, IState to, IPredicate predicate)
        {
            _transitions.Add(new Transition(from, to, predicate));
        }

        public void AddAnyTransition(IState to, IPredicate predicate)
        {
            _anyTransitions.Add(new Transition(null, to, predicate));
        }

        public void Update()
        {
            // Check any transitions first.
            foreach (var transition in _anyTransitions)
            {
                if (transition.Predicate.Evaluate())
                {
                    SetState(transition.To);
                    return;
                }
            }
            // Then check transitions from the current state.
            foreach (var transition in _transitions)
            {
                if (transition.From == _currentState && transition.Predicate.Evaluate())
                {
                    SetState(transition.To);
                    return;
                }
            }
            _currentState?.Update();
        }
    }
}
