using FSM;
using UnityEngine;

namespace FSM
{
    public interface ICondition
    {
        bool EvaluateCondition();
    }
}

namespace Conditions
{
    public class VectorEquality : ICondition
    {
        private bool _equality;
        private Vector2 _lhs;
        private Vector2 _rhs;

        public VectorEquality(Vector2 lhs, Vector2 rhs, bool equality)
        {
            _equality = equality;
        }

        public bool EvaluateCondition()
        {
            return (_lhs == _rhs) == _equality;
        }
    }
}