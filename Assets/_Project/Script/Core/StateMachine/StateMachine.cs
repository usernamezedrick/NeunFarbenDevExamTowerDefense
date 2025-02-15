using System;
using System.Collections.Generic; 

namespace NF.Main.Core
{
    [System.Serializable]
    public class StateMachine
    {
        private StateNode _currentState;
        private Type _type;
        private Dictionary<Type, StateNode> _nodes = new Dictionary<Type, StateNode>();
        private HashSet<ITransition> _anyTransitions = new HashSet<ITransition>();
        private bool _isNewState = true;

        // Check for any transitions that should happen and update the current state's logic
        public void Update()
        {
            var transition = GetTransition();
            if (transition != null)
            {
                ChangeState(transition.To);
            }

            _currentState.State?.Update();
        }

        // Update the current state's physics-related logic
        public void FixedUpdate()
        {
            _currentState.State?.FixedUpdate();
        }

        // Switching the state to the provided one
        public void SetState(IState state)
        {
            _type = state.GetType();
            _currentState = _nodes[_type];
            _currentState.State.OnEnter(); 
        }

        // To add transition between two states with a given condition
        public void AddTransition(IState from, IState to, IPredicate condition)
        {
            StateNode stateNode = GetOrAddNode(from);
            IState transitionState = GetOrAddNode(to).State;
            stateNode.AddTransition(transitionState, condition);
        }

        // To add transition from any state to a specific state with a condition
        public void AddAnyTransition(IState to, IPredicate condition)
        {
            IState transitionState = GetOrAddNode(to).State;
            _anyTransitions.Add(new Transition(transitionState, condition));
        }

        // Changes the current state to a new one
        private void ChangeState(IState state)
        {
            _isNewState = state != _currentState.State;
            if (!_isNewState)
            {
                return;
            }

            var stateType = state.GetType();

            var previousState = _currentState.State;
            var nextState = _nodes[stateType].State;

            previousState?.OnExit();
            nextState?.OnEnter();

            _type = stateType;
            _currentState = _nodes[_type]; 
        }

        // To check if any transitions should occur, either from any transition or the current state
        private ITransition GetTransition()
        {
            foreach (var transition in _anyTransitions)
            {
                if (transition.Condition.Evaluate())
                {
                    return transition;
                }
            }

            foreach (var transition in _currentState.Transitions)
            {
                if (transition.Condition.Evaluate())
                {
                    return transition;
                }
            }

            return null;
        }

        // To retrieve or create a StateNode for a given state
        private StateNode GetOrAddNode(IState state)
        {
            var key = state.GetType();
            var node = _nodes.GetValueOrDefault(key);
            if (node == null)
            {
                node = new StateNode(state);
                _nodes.Add(key, node);
            }

            return node;
        }

        // Container that contains the state and its transitions
        private class StateNode
        {
            public IState State { get; }
            public HashSet<ITransition> Transitions { get; }

            public StateNode(IState state)
            {
                State = state;
                Transitions = new HashSet<ITransition>();
            }

            // To add transition from this state to another state with a condition
            public void AddTransition(IState to, IPredicate condition)
            {
                Transitions.Add(new Transition(to, condition));
            }
        }
    }
}
