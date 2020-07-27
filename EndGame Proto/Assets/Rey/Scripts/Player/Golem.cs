using UnityEngine;
using System.Collections.Generic;

public class Golem : MonoBehaviour, IRequireInput
{
    [SerializeField] private float _speed;
    [SerializeField] private float _angularSpeed;
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

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _movement = new VelocityBasedMovement(_rb, _speed);

        _thisTransform = transform;
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        ComputeAxes();
        _movement.Move(_heading);
        _anim.SetFloat("Speed", _rb.velocity.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        if (_currentState == STATE.ACTIVE)
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
}