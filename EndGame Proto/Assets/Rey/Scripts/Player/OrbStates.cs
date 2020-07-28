using FSM;
using UnityEngine;

namespace OrbStates
{
    public class IdleState : State
    {
        private Orb _orb;

        public IdleState(Orb orb) : base("Idle State")
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

    public class RollingState : State
    {
        private Orb _orb;

        public RollingState(Orb orb) : base("Rolling State")
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

    public class MountedState : State
    {
        private Orb _orb;

        public MountedState(Orb orb) : base("Mounted State")
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
            
        }

        public override void UpdatePhysics()
        {
            
        }
    }
}