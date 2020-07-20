using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public enum STATE
{
	FREE,
	PUSHING,
	LIFTING
}

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float _speed;
	[SerializeField] private float _angularSpeed;
	private Rigidbody _rb;
	private bool _holding = false;
	private bool _lifting = false; 

	private STATE _currentState = STATE.FREE; 

	private Transform _cameraTransform;
	private Vector3 _heading; 
	private Vector3 _forward;
	private Vector3 _right;
	private Vector3 _input;
	private float _angle;

	//private Vector3 _mosuePos;
	////Transform _trans;
	//private Vector3 _objPos;

	private Rigidbody _heldObject;
	private Vector3 _heldObjectNormal;
	private Vector3 _heldOjectPosition; 

	private int _layerMask = 1 << 8;


	// Start is called before the first frame update
	void Start()
	{
		_rb = GetComponent<Rigidbody>();
		_cameraTransform = Camera.main.transform;
	}

	// Update is called once per frame
	void Update()
	{
		float movementHorizontal = Input.GetAxis("Horizontal");
		float movementVertical = Input.GetAxis("Vertical");

		_input = new Vector3(movementHorizontal, 0.0f, movementVertical);
		//_mosuePos = Input.mousePosition;
		//_mosuePos.z = Vector3.Distance(transform.position, Camera.main.transform.position); //The distance between the camera and object
		//_objPos = Camera.main.WorldToScreenPoint(transform.position);
		//_mosuePos.x = _mosuePos.x - _objPos.x;
		//_mosuePos.y = _mosuePos.y - _objPos.y;
		//_angle = Mathf.Atan2(_mosuePos.y, _mosuePos.x) * Mathf.Rad2Deg;
		//transform.rotation = Quaternion.Euler(new Vector3(0, 90 - _angle, 0));

		_angle = _cameraTransform.rotation.eulerAngles.y;
		_forward = Quaternion.AngleAxis(_angle, Vector3.up) * Vector3.forward;
		_right = Vector3.Cross(Vector3.up, _forward);

		_heading = _input.normalized.x * _right + _input.normalized.z * _forward;

		Quaternion targetRotation = Quaternion.LookRotation(_forward, Vector3.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _angularSpeed * Time.fixedDeltaTime);

		RaycastHit hit;

		//Pushing 
		if (Input.GetKeyDown(KeyCode.E))
		{
			if (_holding == false)
			{
				if (Physics.Raycast(transform.position - new Vector3(0, 0.5f, 0), transform.forward, out hit, 2, _layerMask))
				{
					_heldObject = hit.collider.GetComponent<Rigidbody>();
					_heldObject.isKinematic = false;
					_heldObjectNormal = hit.normal;
					_currentState = STATE.PUSHING;
					_holding = true;
				}
			}
			else if (_holding == true)
			{
				_holding = false;
				_heldObject.isKinematic = true;
				_currentState = STATE.FREE;
			}
		}

		if (Input.GetKeyDown(KeyCode.Q))
		{
			if (_lifting == false)
			{
				if (Physics.Raycast(transform.position - new Vector3(0, 0.5f, 0), transform.forward, out hit, 2, _layerMask))
				{
					_heldObject = hit.collider.GetComponent<Rigidbody>();
					_heldObject.isKinematic = true; 
					_heldOjectPosition = _heldObject.GetComponent<Transform>().position;
					_heldObject.GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z); 
					_currentState = STATE.LIFTING;
					_lifting = true;
				}
			}
			else if (_lifting == true)
			{
				_lifting = false;
				_heldObject.isKinematic = false;
				_heldObject.GetComponent<Transform>().position = _heldOjectPosition;
				_currentState = STATE.FREE; 
			}
		}
	}

	private void FixedUpdate()
	{
		//Never do input in fixed update 



		switch(_currentState)
		{
			case STATE.FREE:
				{
					_rb.constraints = RigidbodyConstraints.FreezeRotation; 
					_rb.velocity = _heading * _speed; 
					break; 
				}
			case STATE.PUSHING:
				{
					_rb.velocity = _input.z * -_heldObjectNormal * _speed;
					_heldObject.velocity = _rb.velocity; 
					_rb.constraints = RigidbodyConstraints.FreezeRotation; 
					break;
				}
			case STATE.LIFTING:
				{
					_rb.constraints = RigidbodyConstraints.FreezeAll; 
					break; 
				}
		}
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
