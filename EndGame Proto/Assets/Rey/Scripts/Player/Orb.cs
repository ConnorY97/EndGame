﻿using OrbStates;
using UnityEngine;

public class Orb : MonoBehaviour, IRequireInput
{
    [SerializeField] private float _speed;
    [SerializeField] private float _angularSpeed;
    private IMovement _movement;

    private InputData _inputData;

    private float _angle;
    private Vector3 _forward;
    private Vector3 _right;
    private Vector3 _currentHeading;

    private Golem _currentGolem;
    [SerializeField] private float _golemSearchRadius;
    [SerializeField] private Vector3 _attachmentOffset;

    private Rigidbody _rb;
    private Transform _thisTransform;
    private Transform _cameraTransform;
    [SerializeField] private GameObject _CMVirtualCamera;

    private FSM.FSM _fsm;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _movement = new VelocityBasedMovement(_rb, _speed);

        _thisTransform = transform;
        _cameraTransform = Camera.main.transform;
    }

    private void Start()
    {
        InitialiseFSM();

        DebugWindow.Inspect(() => "Current State: " + _fsm.GetCurrentState().debugName);
        DebugWindow.Inspect(() => "Current Heading: " + _currentHeading.ToString());
    }

    private void Update()
    {
        ComputeAxes();

        //if (Input.GetKeyDown(KeyCode.Space))
        //    _mounted = !_mounted;

        _fsm.HandleTransitions();
        _fsm.UpdateLogic();
    }

    private void FixedUpdate()
    {
        _fsm.UpdatePhysics();
    }

    private void InitialiseFSM()
    {
        _fsm = new FSM.FSM();

        FSM.State groundedSuperState = new GroundedState(this, null);
        FSM.State idleState = new IdleState(this, groundedSuperState);
        FSM.State rollingState = new RollingState(this, groundedSuperState);
        FSM.State mountedState = new MountedState(this, null);

        FSM.Condition isIdle = new FSM.Condition(() =>
        {
            return _currentHeading == Vector3.zero;
        });

        FSM.Condition isRolling = new FSM.Condition(() =>
        {
            return _currentHeading != Vector3.zero;
        });

        //FSM.Condition isMounted = new Condition(() =>
        //{
        //    return _mounted;
        //});

        //FSM.Condition unMounted = new Condition(() =>
        //{
        //    return !_mounted;
        //});

        //FSM.Transition groundedToMounted = new Transition(mountedState, isMounted);
        FSM.Transition groundedToIdle = new FSM.Transition(idleState, isIdle);
        FSM.Transition groundedToRolling = new FSM.Transition(rollingState, isRolling);

        //FSM.Transition mountedToGrounded = new Transition(groundedSuperState, unMounted);

        _fsm.AddState(groundedSuperState, /*groundedToMounted,*/ groundedToIdle, groundedToRolling);
        _fsm.AddState(idleState);
        _fsm.AddState(rollingState);

        //_fsm.AddState(mountedState, mountedToGrounded);

        _fsm.SetDefaultState(groundedSuperState);
    }

    private void ComputeAxes()
    {
        _angle = _cameraTransform.rotation.eulerAngles.y;
        _forward = Quaternion.AngleAxis(_angle, Vector3.up) * Vector3.forward;
        _right = Vector3.Cross(Vector3.up, _forward);

        _currentHeading = _inputData.normalisedInput.x * _right + _inputData.normalisedInput.y * _forward;
    }

    public void Orientate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_forward, Vector3.up);
        _thisTransform.rotation = Quaternion.Slerp(_thisTransform.rotation, targetRotation, _angularSpeed * Time.fixedDeltaTime);
    }

    public void Move()
    {
        _movement.Move(_currentHeading);
    }

    public void ResetState()
    {
        _movement.Move(Vector3.zero);
    }

    // Interfaces
    public void SetInputData(InputData data)
    {
        if (_currentGolem != null)
            _currentGolem.SetInputData(data);
        else
            _inputData = data;
    }
}