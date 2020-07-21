using UnityEngine;

public class Head : MonoBehaviour, IRequireInput
{
    [SerializeField] private float _speed = 0;
    [SerializeField] private float _angularSpeed = 0;
    private IMovement _movement;

    private InputData _inputData;

    private float _angle;
    private Vector3 _forward;
    private Vector3 _right;
    private Vector3 _heading;

    private Rigidbody _rb;
    private Animator _anim;

    private Transform _thisTransform;
    private Transform _cameraTransform;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _movement = new VelocityBasedMovement(_rb, _speed);

        _thisTransform = transform;
        _cameraTransform = Camera.main.transform; 
    }

    private void ComputeAxes()
    {
        _angle = _cameraTransform.rotation.eulerAngles.y;
        _forward = Quaternion.AngleAxis(_angle, Vector3.up) * Vector3.forward;
        _right = Vector3.Cross(Vector3.up, _forward);

        _heading = _inputData.normalisedInput.x * _right + _inputData.normalisedInput.y * _forward;
    }

    private void Update()
    {
        ComputeAxes();

        _movement.Move(_heading);
        //_anim.SetFloat("Speed", _rb.velocity.sqrMagnitude);
    }

    private void FixedUpdate()
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