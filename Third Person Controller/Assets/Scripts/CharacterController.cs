using Cinemachine;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    [SerializeField] private float _speed;
    [SerializeField] private float _angularSpeed;

    [SerializeField] private CinemachineBrain _cinemachineBrain;
    private Transform _cameraTransform;
    private Vector3 _forward;
    private Vector3 _right;
    private Vector2 _input;
    private Vector2 _inputNormalized;
    private float _angle;
    
    private Rigidbody _rb;
    private Animator _anim;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _inputNormalized = _input.normalized;
        _angle = _cameraTransform.rotation.eulerAngles.y;
        _forward = Quaternion.AngleAxis(_angle, Vector3.up) * Vector3.forward;
        _right = Vector3.Cross(Vector3.up, _forward);
        Debug.DrawRay(transform.position, _forward * 5f, Color.blue);
        Debug.DrawRay(transform.position, _right * 5f, Color.red);

        _anim.SetFloat("Speed", _rb.velocity.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        Vector3 velocity = ((_forward * _inputNormalized.y) + (_right * _inputNormalized.x)) * _speed;
        _rb.velocity = velocity;

        if (_rb.velocity.sqrMagnitude != 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_rb.velocity.normalized, Vector3.up);
            _rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, _angularSpeed * Time.deltaTime));
        }
    }
}
