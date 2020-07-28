namespace FSM
{
    public class Condition
    {
        public delegate bool ConditionFunc();
        private ConditionFunc _func;

        public Condition(ConditionFunc func)
        {
            _func = func;
        }

        public bool EvaluateCondition()
        {
            return _func();
        }
    }
}