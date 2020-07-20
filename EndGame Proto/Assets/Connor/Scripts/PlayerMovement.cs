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
	[SerializeField] private float _maxSpeed = 100.0f;
	[SerializeField] private float _angularSpeed;
	private Rigidbody _rb;
	private bool _holding = false;
	private bool _lifting = false;

	private STATE _currentState = STATE.FREE; 

	private Transform _cameraTransform;
	private Vector3 _forward;
	private Vector3 _right;
	private Vector2 _input;
	private Vector2 _inputNormalized;
	private float _angle;

	private Vector3 _mosuePos;
	//Transform _trans;
	private Vector3 _objPos;

	private Transform _liftedObject;
	private Vector3 _originalPosition;

	private Rigidbody _heldObject;

	private Vector3 _heldObjectNormal;

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
		_mosuePos = Input.mousePosition;
		_mosuePos.z = Vector3.Distance(transform.position, Camera.main.transform.position); //The distance between the camera and object
		_objPos = Camera.main.WorldToScreenPoint(transform.position);
		_mosuePos.x = _mosuePos.x - _objPos.x;
		_mosuePos.y = _mosuePos.y - _objPos.y;
		_angle = Mathf.Atan2(_mosuePos.y, _mosuePos.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(new Vector3(0, 90 - _angle, 0));

		RaycastHit hit;

		Debug.DrawRay(transform.position - new Vector3(0, 0.05f, 0), transform.forward * 2, Color.white);

		if (Input.GetKeyDown(KeyCode.E))
		{
			if (_holding == false)
			{
				if (Physics.Raycast(transform.position - new Vector3(0, 0.5f, 0), transform.forward, out hit, 2, _layerMask))
				{
					_heldObject = hit.collider.GetComponent<Rigidbody>();
					_heldObject.isKinematic = false;
					_heldObjectNormal = hit.normal;
					//hit.collider.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity;
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
	}

	private void FixedUpdate()
	{
		//Never do input in fixed update 



		float movementHorizontal = Input.GetAxis("Horizontal");
		float movementVertical = Input.GetAxis("Vertical");

		Vector3 input = new Vector3(movementHorizontal, 0.0f, movementVertical);



		switch(_currentState)
		{
			case STATE.FREE:
				{
					_rb.constraints = RigidbodyConstraints.None; 
					_rb.velocity = input.normalized * _speed; 
					break; 
				}
			case STATE.PUSHING:
				{
					_heldObject.velocity = _rb.velocity; 
					_rb.velocity = input.z * -_heldObjectNormal * _speed;
					_rb.constraints = RigidbodyConstraints.FreezeRotation; 
					break;
				}
		}


		//if (Input.GetKey(KeyCode.E))
		//{
		//	if (Physics.Raycast(transform.position - new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.forward), out hit, 5, _layerMask))
		//	{
		//		//Debug.DrawRay(transform.position - new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

		//		_rb.velocity = -hit.normal * movement.z * _speed;
		//	}
		//	else
		//	{
		//		//Debug.DrawRay(transform.position - new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.forward) * 1000, Color.white);
		//		Debug.Log("Did not Hit");
		//	}
		//}

		//if (Input.GetKeyDown(KeyCode.Q))
		//{
		//	if (Physics.Raycast(transform.position - new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.forward), out hit, 5, _layerMask))
		//	{
		//		if (!_lifting)
		//		{
		//			_liftedObject = hit.collider.GetComponent<Transform>();
		//			_originalPosition = _liftedObject.position; 
		//			_liftedObject.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
		//			_liftedObject.GetComponent<Rigidbody>().isKinematic = false;
		//			_rb.constraints = RigidbodyConstraints.FreezeAll;
		//			_lifting = true;
		//		}
		//	}
		//	else if (_lifting)
		//	{
		//		_liftedObject.position = _originalPosition; 
		//		_liftedObject.GetComponent<Rigidbody>().isKinematic = true;
		//		_rb.constraints = RigidbodyConstraints.None;
		//		_lifting = false;
		//	}
		//}



		//if (!_holding)
		//	_rb.velocity = input.normalized * _speed;
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
