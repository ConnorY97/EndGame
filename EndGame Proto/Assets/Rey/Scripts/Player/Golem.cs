using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using GolemStates;

public class Golem : MonoBehaviour, IRequireInput
{
	[SerializeField] private float _speed = 0;
	[SerializeField] private float _angularSpeed = 0;
	private IMovement _movement;

	private InputData _inputData;

	private Vector3 _forward;
	private Vector3 _right;
	private Vector3 _heading;

	private FSM.FSM _fsm;

	private Rigidbody _rb;
	private Animator _anim;

	private Transform _thisTransform;
	private Transform _cameraTransform;
	[SerializeField] private GameObject _CMVirtualCamera;

	private bool _pushingInput;
	private bool _liftingInput;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();
		_anim = GetComponent<Animator>();
		_movement = new VelocityBasedMovement(_rb, _speed);

		_thisTransform = transform;
		_cameraTransform = Camera.main.transform;

		InitaliseStateMachine(); 
	}

	private void Update()
	{
		ComputeAxes();
		_movement.Move(_heading);
		_anim.SetFloat("Speed", _rb.velocity.sqrMagnitude);

		if (Input.GetKeyDown(KeyCode.E))		
			_pushingInput = !_pushingInput;
		if (Input.GetKeyDown(KeyCode.Q))
			_liftingInput = !_liftingInput; 

	}

	private void FixedUpdate()
	{
		Orientate();
	}

	private void InitialiseFSM()
	{

	}

	private void ComputeAxes()
	{
		float _angle = _cameraTransform.rotation.eulerAngles.y;
		_forward = Quaternion.AngleAxis(_angle, Vector3.up) * Vector3.forward;
		_right = Vector3.Cross(Vector3.up, _forward);

		_heading = _inputData.normalisedInput.x * _right + _inputData.normalisedInput.y * _forward;
	}

	private void Orientate()
	{
		Quaternion targetRotation = Quaternion.LookRotation(_forward, Vector3.up);
		_thisTransform.rotation = Quaternion.Slerp(_thisTransform.rotation, targetRotation, _angularSpeed * Time.fixedDeltaTime);
	}


	// Interfaces
	public void SetInputData(InputData data)
	{
		_inputData = data;
	}

	private void InitaliseStateMachine()
	{
		_fsm = new FSM.FSM();

		//Initialising super states---------------------------------------------------------------------
		//GolemStates.GroundedState groundedState = new GolemStates.GroundedState(this, null);
		//GolemStates.InteractingState interactingState = new GolemStates.InteractingState(this, null);
		////Initialising sub states-----------------------------------------------------------------------
		//GolemStates.IdleState idleState = new GolemStates.IdleState(this, groundedState);
		//GolemStates.WalkingState walkingState = new GolemStates.WalkingState(this, groundedState);
		//GolemStates.LiftingState liftingState = new GolemStates.LiftingState(this, interactingState);
		//GolemStates.PushingState pushState = new GolemStates.PushingState(this, interactingState);
		//Conditions------------------------------------------------------------------------------------
		//FSM.Condition isIdle = new FSM.Condition(() =>
		//{
		//	return _heading == Vector3.zero;
		//});
		//FSM.Condition isWalking = new FSM.Condition(() =>
		//{
		//	return _heading != Vector3.zero;
		//});
		//FSM.Condition canPush = new FSM.Condition(() =>
		//{
		//	return _canPush;
		//});
		//FSM.Condition liftingInput = new FSM.Condition(() =>
		//{
		//	return _liftingInput;
		//});

  //      //Defining transitions--------------------------------------------------------------------------
  //      //grounded super state

  //      FSM.Transition groundedToIdle = new FSM.Transition(idleState, isIdle);
  //      FSM.Transition groundedToWalking = new FSM.Transition(walkingState, isWalking);
  //      //grounded to interacting 
  //      FSM.Transition groundedToInteracting = new FSM.Transition(interactingState, canPush);
  //      FSM.Transition interactingToGrounded = new FSM.Transition(groundedState, pushingInput, liftingInput);
    }
}