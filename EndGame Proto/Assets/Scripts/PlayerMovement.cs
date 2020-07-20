using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _maxSpeed = 100.0f;
    [SerializeField] private float _angularSpeed;
    private Rigidbody _rb;
    private bool _holding = false;

    private Transform _cameraTransform;
    private Vector3 _forward;
    private Vector3 _right;
    private Vector2 _input;
    private Vector2 _inputNormalized;
    private float _angle;

    private Vector3 _mosuePos;
    //Transform _trans;
    private Vector3 _objPos;
     

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        _mosuePos = Input.mousePosition;
        _mosuePos.z = Vector3.Distance(transform.position, Camera.main.transform.position); //The distance between the camera and object
        _objPos = Camera.main.WorldToScreenPoint(transform.position);
        _mosuePos.x = _mosuePos.x - _objPos.x;
        _mosuePos.y = _mosuePos.y - _objPos.y;
        _angle = Mathf.Atan2(_mosuePos.y, _mosuePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 90 - _angle, 0));
    }

    private void FixedUpdate()
    {
        int _layerMask = 1 << 8;
        _holding = false;
        RaycastHit hit;
        float movementHorizontal = Input.GetAxis("Horizontal");
        float movementVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(movementHorizontal, 0.0f, movementVertical);

        if (Input.GetKey(KeyCode.E))
        {
            if (Physics.Raycast(transform.position - new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.forward), out hit, 5, _layerMask))
            { 
                Debug.DrawRay(transform.position - new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                hit.collider.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity;
                _holding = true;
                _rb.velocity = -hit.normal * movement.z * _speed; 
            }
            else
            {
                Debug.DrawRay(transform.position - new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                Debug.Log("Did not Hit");
            }
        }

		if (!_holding)
            _rb.velocity = movement.normalized * _speed; 
    }

    /*
     *    [SerializeField] private float _speed;
    [SerializeField] private float _angularSpeed;

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

        Vector3 velocity = ((_forward * _inputNormalized.y) + (_right * _inputNormalized.x)) * _speed;
        _rb.velocity = velocity;

        _anim.SetFloat("Speed", _rb.velocity.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_forward, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _angularSpeed * Time.fixedDeltaTime);
    } 
     * 
     */
}
