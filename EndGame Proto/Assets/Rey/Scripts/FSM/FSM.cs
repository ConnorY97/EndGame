using System.Collections.Generic;

namespace FSM
{
	public class FSM
	{
		private List<State> _states;
		private State _currentState;
		private Dictionary<State, Transition[]> _transitionsDict;

		private Transition[] _currentStateTransitions; // The transitions being considered at a given moment.
		private Transition[] _superStateTransitions;

		public FSM()
		{
			_states = new List<State>();
			_transitionsDict = new Dictionary<State, Transition[]>();
		}

		public void SetDefaultState(State defaultState)
		{
			MoveTo(defaultState);
		}

		public void AddState(State state, params Transition[] transitions)
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
			_currentStateTransitions = _transitionsDict[_currentState];

			if (_currentState.superState != null)
				_superStateTransitions = _transitionsDict[_currentState.superState];
			else
				_superStateTransitions = null;

			_currentState.OnEnter();
		}

		public void HandleTransitions()
		{
			State dst = null;
			for (int i = 0; i < _currentStateTransitions.Length; i++)
			{
				dst = _currentStateTransitions[i].EvaluateTransition();

				if (dst != null)
					break;
			}

			if (dst == null)
			{
				for (int i = 0; i < _superStateTransitions.Length; i++)
				{
					dst = _superStateTransitions[i].EvaluateTransition();

					if (dst != null)
						break;
				}
			}

			if (dst != null && dst != _currentState)
				MoveTo(dst);
		}

		public void UpdateLogic()
		{
			if (_currentState.superState != null)
				_currentState.superState.UpdateLogic();
			_currentState.UpdateLogic();
		}

		public void UpdatePhysics()
		{
			if (_currentState.superState != null)
				_currentState.superState.UpdatePhysics();
			_currentState.UpdatePhysics();
		}

		public State GetCurrentState() => _currentState;
	}
}