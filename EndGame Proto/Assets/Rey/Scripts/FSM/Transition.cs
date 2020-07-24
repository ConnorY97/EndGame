namespace FSM
{
    public class Transition
    {
        private State _dstState;
        private ICondition[] _conditions;

        public Transition(State dstState, ICondition[] conditions)
        {
            _dstState = dstState;
            _conditions = conditions;
        }

        public State EvaluateTransition()
        {
            int satisfiedConditions = 0;
            for (int i = 0; i < _conditions.Length; i++)
            {
                if (_conditions[i].EvaluateCondition())
                    satisfiedConditions++;
            }

            if (satisfiedConditions == _conditions.Length)
                return _dstState;

            return null;
        }
    }
}