namespace FSM
{
    public abstract class State
    {
        // Solely for debugging purposes.
        protected string _debugName;
        public string debugName => _debugName;

        protected State _superState;
        public State superState => _superState;

        public State(State superState, string debugName)
        {
            _superState = superState;
            _debugName = debugName;
        }

        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void UpdateLogic();
        public abstract void HandleTransitions();
        public abstract void UpdatePhysics();
    }
}