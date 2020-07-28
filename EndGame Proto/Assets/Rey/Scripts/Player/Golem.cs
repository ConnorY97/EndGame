using UnityEngine;
using GolemStates;
using FSM;

public class Golem : MonoBehaviour, IRequireInput
{
    [SerializeField] private float _speed = 0;
    [SerializeField] private float _angularSpeed = 0;
    private IMovement _movement;

    private InputData _inputData;

    private Vector3 _forwardRelativeToCamera;
    private Vector3 _right;
    private Vector3 _heading;

    private FSM.FSM _fsm;

    [SerializeField] private LayerMask _blockLayer;
    [SerializeField] private float _blockInteractionDistance;
    [SerializeField] private float _distFromBlock;
    private Rigidbody _blockRigidbody;
    private FixedJoint _blockJoint;
    private Vector3 _blockNormal;
    private Vector3 _blockInitialPos;

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

        InitaliseFSM();
    }

    private void Start()
    {
        InitaliseFSM();

        DebugWindow.Inspect(() => { return _fsm.GetCurrentState().debugName; });
        DebugWindow.Inspect(() => { return _heading.ToString(); });
    }

    private void Update()
    {
        ComputeAxes();

        _fsm.UpdateLogic();
        _fsm.HandleTransitions();

        _anim.SetFloat("Speed", _rb.velocity.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        _fsm.UpdatePhysics();
    }

    private void InitaliseFSM()
    {
        _fsm = new FSM.FSM();

        State idleState = new IdleState(this);
        State walkingState = new WalkingState(this);
        State pushingState = new PushingState(this);
        State liftingState = new LiftingState(this);

        _fsm.AddTransition(idleState, walkingState, () => { return _heading != Vector3.zero; });
        _fsm.AddTransition(walkingState, idleState, () => { return _heading == Vector3.zero; });

        // Pushing
        _fsm.AddTransition(idleState, pushingState, () =>
        {
            if (Input.GetKeyDown(KeyCode.E))
                return BeginPushing();

            return false;
        });

        _fsm.AddTransition(pushingState, idleState, () =>
        {
            return Input.GetKeyDown(KeyCode.E);
        });

        // Lifting
        _fsm.AddTransition(idleState, liftingState, () =>
        {
            if (Input.GetKeyDown(KeyCode.Q))
                return Lift();

            return false;
        });

        _fsm.AddTransition(liftingState, idleState, () =>
        {
            return Input.GetKeyDown(KeyCode.Q);
        });

        _fsm.SetDefaultState(idleState);
    }

    private void ComputeAxes()
    {
        float _angle = _cameraTransform.rotation.eulerAngles.y;
        _forwardRelativeToCamera = Quaternion.AngleAxis(_angle, Vector3.up) * Vector3.forward;
        _right = Vector3.Cross(Vector3.up, _forwardRelativeToCamera);

        _heading = _inputData.normalisedInput.x * _right + _inputData.normalisedInput.y * _forwardRelativeToCamera;
    }

    public void Move()
    {
        _movement.Move(_heading);
    }

    public void Orientate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_forwardRelativeToCamera, Vector3.up);
        _thisTransform.rotation = Quaternion.Slerp(_thisTransform.rotation, targetRotation, _angularSpeed * Time.fixedDeltaTime);
    }

    public bool BeginPushing()
    {
        RaycastHit hit;
        if (Physics.Raycast(_thisTransform.position + Vector3.up * 0.5f, _forwardRelativeToCamera, out hit, _blockInteractionDistance, _blockLayer))
        {
            _blockRigidbody = hit.collider.GetComponent<Rigidbody>();
            _blockJoint = hit.collider.GetComponent<FixedJoint>();

            _blockRigidbody.isKinematic = false;
            _blockNormal = hit.normal;

            Vector3 newGolemPos = _blockRigidbody.position + (_blockNormal * _distFromBlock);
            newGolemPos.y = _thisTransform.position.y;

            _thisTransform.position = newGolemPos;
            _thisTransform.rotation = Quaternion.LookRotation(-_blockNormal);

            _blockJoint.connectedBody = _rb;
            return true;
        }

        return false;
    }

    public void Push()
    {
        _movement.Move(_inputData.normalisedInput.y * -_blockNormal);
    }

    public void StopPushing()
    {
        _blockJoint.connectedBody = null;
        _blockRigidbody.isKinematic = true;
        _blockRigidbody = null;
        _blockJoint = null;
    }

    public bool Lift()
    {
        RaycastHit hit;
        if (Physics.Raycast(_thisTransform.position + Vector3.up * 0.5f, _forwardRelativeToCamera, out hit, _blockInteractionDistance, _blockLayer))
        {
            _blockRigidbody = hit.collider.GetComponent<Rigidbody>();
            _blockInitialPos = _blockRigidbody.position;
            _blockRigidbody.position = _thisTransform.position + new Vector3(0, 2.5f, 0);

            return true;
        }

        return false;
    }

    public void StopLifting()
    {
        _blockRigidbody.position = _blockInitialPos;
    }    

    public void ResetState()
    {
        _rb.velocity = Vector3.zero;
    }

    // Interfaces
    public void SetInputData(InputData data)
    {
        _inputData = data;
    }
}