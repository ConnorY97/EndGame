﻿using FSM;
using UnityEngine;

namespace GolemStates
{
	public class IdleState : FSM.State
	{
		private Golem _golem;

		public IdleState(Golem golem) : base("Idle State")
		{
			_golem = golem;
		}

		public override void OnEnter()
		{
			Debug.Log($"Entered {_debugName} State!");
		}

		public override void OnExit()
		{
			_golem.ResetState();
			Debug.Log($"Exited {_debugName} State!");
		}

		public override void UpdateLogic()
		{

		}

        public override void UpdatePhysics()
		{
			
		}
	}

	public class WalkingState : FSM.State
	{
		private Golem _golem;

		public WalkingState(Golem golem) : base("Walking State")
		{
			_golem = golem;
		}

		public override void OnEnter()
		{
			Debug.Log($"Entered {_debugName} State!");
		}

		public override void OnExit()
		{
			_golem.ResetState();
			Debug.Log($"Exited {_debugName} State!");
		}

		public override void UpdateLogic()
		{
			_golem.Move();
		}

		public override void UpdatePhysics()
		{
			_golem.Orientate();
		}
	}

	public class LiftingState : FSM.State
	{
		private Golem _golem;

		public LiftingState(Golem golem) : base("Lifting State")
		{
			_golem = golem;
		}

		public override void OnEnter()
		{
			_golem.SetAnimatorBool("Lifting", true);
			Debug.Log($"Entered {_debugName} State!");
		}

		public override void OnExit()
		{
			_golem.SetAnimatorBool("Lifting", false);
			_golem.StopLifting();
			_golem.ResetState();
			Debug.Log($"Exited {_debugName} State!");
		}

		public override void UpdateLogic()
		{
			_golem.Lift();
		}

		public override void UpdatePhysics()
		{
			
		}
	}

	public class PushingState : FSM.State
	{
		private Golem _golem;

		public PushingState(Golem golem) : base("Pushing State")
		{
			_golem = golem;
		}

		public override void OnEnter()
		{
			_golem.SetAnimatorBool("Pushing", true);
			Debug.Log($"Entered {_debugName} State!");
		}

		public override void OnExit()
		{
			_golem.SetAnimatorBool("Pushing", false);
			_golem.ResetState();
			_golem.StopPushing();
		}

		public override void UpdateLogic()
		{
			_golem.Push();
		}

		public override void UpdatePhysics()
		{
			
		}
	}
}