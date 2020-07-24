using Cinemachine;
using FSM;
using OrbStates;
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
    [SerializeField] private CinemachineFreeLook _CM_VirtualCamera;

    private FSM.FSM _fsm;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _movement = new VelocityBasedMovement(_rb, _speed);

        _thisTransform = transform;
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _currentGolem == null)
        {
            Golem[] golems = FindObjectsOfType<Golem>();

            for (int i = 0; i < golems.Length; i++)
            {
                if (Vector3.Distance(golems[i].transform.position, _thisTransform.position) <= _golemSearchRadius)
                    JumpInGolem(golems[i]);
            }
        }

        if (_currentGolem != null)
            _thisTransform.position = _currentGolem.transform.position + _attachmentOffset;

        ComputeAxes();
    }

    private void InitialiseFSM()
    {
        _fsm = new FSM.FSM();

        FSM.State groundedSuperState = new GroundedState(this, null);
        FSM.State idleState = new IdleState(this, groundedSuperState);
        FSM.State rolingState = new RollingState(this, groundedSuperState);


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

    private void JumpInGolem(Golem golem)
    {
        _CM_VirtualCamera.LookAt = null;
        _CM_VirtualCamera.Follow = null;

        _currentGolem = golem;
        _currentGolem.Toggle(true);
    }

    private void JumpOutGolem()
    {

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