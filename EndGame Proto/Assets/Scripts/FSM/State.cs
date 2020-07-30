namespace FSM
{
    public abstract class State
    {
        // Solely for debugging purposes.
        protected string _debugName;
        public string debugName => _debugName;

        public State(string debugName)
        {
            _debugName = debugName;
        }

        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void UpdateLogic();
        public abstract void UpdatePhysics();
    }
}