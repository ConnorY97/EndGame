using FSM;
using UnityEngine;

namespace OrbStates
{
    // Super-state for states involve being grounded.
    public class GroundedState : FSM.State
    {
        public GroundedState(Orb orb, FSM.State superState) : base(superState, "Grounded State")
        {

        }

        public override void OnEnter()
        {
            
        }

        public override void OnExit()
        {
            
        }

        public override void UpdateLogic()
        {
            
        }

        public override void UpdatePhysics()
        {
            
        }
    }

    public class IdleState : FSM.State
    {
        private Orb _orb;

        public IdleState(Orb orb, FSM.State superState) : base(superState, "Idle")
        {
            _orb = orb;
        }

        public override void OnEnter()
        {
            Debug.Log("Entered Idle State!");
        }

        public override void OnExit()
        {
            _orb.ResetState();
            Debug.Log("Exiting Idle State!");
        }

        public override void UpdateLogic()
        {
            
        }

        public override void UpdatePhysics()
        {
            
        }
    }

    public class RollingState : FSM.State
    {
        private Orb _orb;

        public RollingState(Orb orb, FSM.State superState) : base(superState, "Rolling")
        {
            _orb = orb;
        }

        public override void OnEnter()
        {
            Debug.Log($"Entered {_debugName} State!");
        }

        public override void OnExit()
        {
            _orb.ResetState();
            Debug.Log($"Exited {_debugName} State!");
        }

        public override void UpdateLogic()
        {
            _orb.Move();
        }

        public override void UpdatePhysics()
        {
            _orb.Orientate();
        }
    }
}