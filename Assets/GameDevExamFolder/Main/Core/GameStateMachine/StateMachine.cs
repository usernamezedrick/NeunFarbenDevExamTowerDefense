using System.Collections.Generic;

namespace NF.Main.Core.GameStateMachine
{
    public class StateMachine
    {
        private IState _currentState;
        private readonly List<Transition> _transitions = new List<Transition>();
        private readonly List<Transition> _anyTransitions = new List<Transition>();

        /// <summary>
        /// Sets the state if the new state's type is different from the current state's type.
        /// </summary>
        public void SetState(IState newState)
        {
            // If the current state exists and its type is the same as the new state's type, do nothing.
            if (_currentState != null && _currentState.GetType() == newState.GetType())
            {
                return;
            }
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
            // Evaluate any transitions first.
            foreach (var transition in _anyTransitions)
            {
                if (transition.Predicate.Evaluate())
                {
                    SetState(transition.To);
                    return;
                }
            }
            // Evaluate transitions from the current state.
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
