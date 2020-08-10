using System;
using System.Collections.Generic;

namespace FSM
{
	public class FSM
	{
		private State _currentState;
		private Dictionary<State, List<Transition>> _transitions;

		private List<Transition> _currentTransitions; // The transitions being considered at a given moment.

		public FSM()
		{
			_transitions = new Dictionary<State, List<Transition>>();
		}

		public void SetDefaultState(State defaultState)
		{
			MoveTo(defaultState);
		}

		public void AddTransition(State from, State to, Func<bool> condition)
        {
			if (_transitions.TryGetValue(from, out var transitions) == false)
            {
                transitions = new List<Transition>();
				_transitions[from] = transitions;
            }

			transitions.Add(new Transition(to, condition));
        }

		public void MoveTo(State state)
		{
			if (_currentState != null)
				_currentState.OnExit();

			_currentState = state;
			_currentTransitions = _transitions[_currentState];

			_currentState.OnEnter();
		}

		public void HandleTransitions()
		{
			for (int i = 0; i < _currentTransitions.Count; i++)
			{
				if (_currentTransitions[i].condition())
				{
					MoveTo(_currentTransitions[i].to);
					return;
				}
			}
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

		private class Transition
		{
            public State to;
			public Func<bool> condition;

			public Transition(State to, Func<bool> condition)
			{
				this.to = to;
				this.condition = condition;
			}
		}
	}
}