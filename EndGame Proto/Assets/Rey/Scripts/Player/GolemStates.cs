using FSM;
using UnityEngine;

namespace GolemStates
{
	public class GroundedState : FSM.State
	{
		public GroundedState(Golem golem, FSM.State superState) : base(superState, "Grounded State")
		{

		}

		public override void OnEnter()
		{
			throw new System.NotImplementedException();
		}

		public override void OnExit()
		{
			throw new System.NotImplementedException();
		}

		public override void UpdateLogic()
		{
			throw new System.NotImplementedException();
		}

		public override void UpdatePhysics()
		{
			throw new System.NotImplementedException();
		}
	}

	public class InteractingState : FSM.State
	{
		public InteractingState(Golem golem, FSM.State superState) : base(superState, "Interacting State")
		{

		}

		public override void OnEnter()
		{
			throw new System.NotImplementedException();
		}

		public override void OnExit()
		{
			throw new System.NotImplementedException();
		}

		public override void UpdateLogic()
		{

		}

		public override void UpdatePhysics()
		{
			throw new System.NotImplementedException();
		}
	}

	public class IdleState : FSM.State
	{
		public IdleState(Golem golem, FSM.State superState) : base(superState, "Idle State")
		{

		}

		public override void OnEnter()
		{
			throw new System.NotImplementedException();
		}

		public override void OnExit()
		{
			throw new System.NotImplementedException();
		}

		public override void UpdateLogic()
		{
			throw new System.NotImplementedException();
		}

		public override void UpdatePhysics()
		{
			throw new System.NotImplementedException();
		}
	}

	public class WalkingState : FSM.State
	{
		public WalkingState(Golem golem, FSM.State superState) : base(superState, "Walking State")
		{

		}

		public override void OnEnter()
		{
			throw new System.NotImplementedException();
		}

		public override void OnExit()
		{
			throw new System.NotImplementedException();
		}

		public override void UpdateLogic()
		{
			throw new System.NotImplementedException();
		}

		public override void UpdatePhysics()
		{
			throw new System.NotImplementedException();
		}
	}

	public class LiftingState : FSM.State
	{
		public LiftingState(Golem golem, FSM.State superState) : base(superState, "Carrying State")
		{

		}

		public override void OnEnter()
		{
			throw new System.NotImplementedException();
		}

		public override void OnExit()
		{
			throw new System.NotImplementedException();
		}

		public override void UpdateLogic()
		{
			throw new System.NotImplementedException();
		}

		public override void UpdatePhysics()
		{
			throw new System.NotImplementedException();
		}
	}

	public class PushingState : FSM.State
	{
		public PushingState(Golem golem, FSM.State superState) : base(superState, "Push State")
		{

		}

		public override void OnEnter()
		{
			throw new System.NotImplementedException();
		}

		public override void OnExit()
		{
			throw new System.NotImplementedException();
		}

		public override void UpdateLogic()
		{
			throw new System.NotImplementedException();
		}

		public override void UpdatePhysics()
		{
			throw new System.NotImplementedException();
		}
	}
}

