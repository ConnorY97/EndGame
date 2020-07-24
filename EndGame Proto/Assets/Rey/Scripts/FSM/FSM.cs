using System.Collections.Generic;

namespace FSM
{
    public class FSM
    {
        private List<State> _states;
        private State _currentState;
        private Dictionary<State, Transition[]> _transitionsDict;

        private Transition[] _currentTransitions; // The transitions being considered at a given moment.

        public FSM()
        {
            _states = new List<State>();
            _transitionsDict = new Dictionary<State, Transition[]>();
        }

        public void AddState(State state, Transition[] transitions)
        {
            if (_transitionsDict.ContainsKey(state))
                return;

            _states.Add(state);
            _transitionsDict.Add(state, transitions);
        }

        private void MoveTo(State state)
        {
            if (_currentState != null)
                _currentState.OnExit();

            _currentState = state;
            _currentState.OnEnter();
        }

        public void UpdateLogic()
        {
            _currentState.UpdateLogic();
        }

        public void UpdatePhysics()
        {
            _currentState.UpdatePhysics();
        }

        public State GetCurrentState() => _currentState;
    }
}