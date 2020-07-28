using UnityEngine;

public enum STATE
{
	FREE,
	PUSHING,
	LIFTING
}

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float _speed = 0;
	[SerializeField] private float _angularSpeed = 0;
	[SerializeField] private float _rayLength = 0;

	public GameObject handPosition; 

	private Rigidbody _rb;
	private Animator _animator; 

	private bool _pushing = false;
	private bool _lifting = false;


	private STATE _currentState = STATE.FREE; 

	private Transform _cameraTransform;
	private Vector3 _heading; 
	private Vector3 _forward;
	private Vector3 _right;
	private Vector3 _input;
	private float _angle;

	private Rigidbody _interactedObject;
	private Vector3 _ioNormal;
	private Vector3 _ioStartPosition; 

	private int _layerMask = 1 << 8;

	private Vector3 _rayPos;



	// Start is called before the first frame update
	void Start()
	{
		_rb = GetComponent<Rigidbody>();
		_animator = GetComponent<Animator>(); 
		_cameraTransform = Camera.main.transform;
	}

	// Update is called once per frame
	void Update()
	{
		_rayPos = transform.position + new Vector3(0, 0.25f, 0);

		float movementHorizontal = Input.GetAxis("Horizontal");
		float movementVertical = Input.GetAxis("Vertical");

		_input = new Vector3(movementHorizontal, 0.0f, movementVertical);

		_angle = _cameraTransform.rotation.eulerAngles.y;
		_forward = Quaternion.AngleAxis(_angle, Vector3.up) * Vector3.forward;
		_right = Vector3.Cross(Vector3.up, _forward);

		_heading = _input.normalized.x * _right + _input.normalized.z * _forward;

		//RaycastHit hit;
		//Pushing 
		if (Input.GetKeyDown(KeyCode.E))
		{
			Push(); 
		}

		//Lifting 
		if (Input.GetKeyDown(KeyCode.Q))
		{
			Lift(); 
		}


		switch (_currentState)
		{
			case STATE.FREE:
				{
					_rb.constraints = RigidbodyConstraints.FreezeRotation;
					_rb.velocity = _heading * _speed;
					break;
				}
			case STATE.PUSHING:
				{
					_rb.velocity = _input.z * -_ioNormal * _speed;
					_rb.constraints = RigidbodyConstraints.FreezeRotation;
					break;
				}
			case STATE.LIFTING:
				{
					_rb.constraints = RigidbodyConstraints.FreezeAll;
					break;
				}
		}

		//Animation
		_animator.SetFloat("Speed", _rb.velocity.magnitude);
		_animator.SetBool("Pushing", _pushing);
		_animator.SetBool("Lifting", _lifting); 
		_animator.SetFloat("Direction", Mathf.RoundToInt(Vector3.Dot(_rb.velocity.normalized, transform.forward)));
	}

	private void FixedUpdate()
	{
		switch (_currentState)
		{
			case STATE.FREE:
			{
					Quaternion targetRotation = Quaternion.LookRotation(_forward, Vector3.up);
					transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _angularSpeed * Time.fixedDeltaTime);
					break;
			}
		}
	}

	/// <summary>
	/// Ray casting for an object to be set as _heldObject,
	///	then allows for the object to be moved around the scene on a single axis 
	/// </summary>
	public void Push()
	{
		RaycastHit hit;
		if (_lifting == false)
		{
			if (_pushing == false)
			{
				if (Physics.Raycast(_rayPos, _forward, out hit, _rayLength, _layerMask))
				{
					_interactedObject = hit.collider.GetComponent<Rigidbody>();
					_ioStartPosition = _interactedObject.transform.position;
					_ioStartPosition.y = transform.position.y;
					_interactedObject.isKinematic = false;
					_ioNormal = hit.normal;
					_rb.transform.position = _ioStartPosition + (_ioNormal * 1.2f);
					transform.rotation = Quaternion.LookRotation(-_ioNormal);
					_interactedObject.GetComponent<FixedJoint>().connectedBody = _rb;
					_pushing = true;
					_currentState = STATE.PUSHING;
				}
			}
			else if (_pushing == true)
			{
				_interactedObject.GetComponent<FixedJoint>().connectedBody = null;
				_interactedObject.isKinematic = true;
				_interactedObject = null;
				_ioNormal = Vector3.zero;
				_ioStartPosition = Vector3.zero; 
				_pushing = false;
				_currentState = STATE.FREE;
			}
		}
	}

	/// <summary>
	/// Ray casting for an object to be set as _heldobject
	/// then allows for the object to be lifted into the air 
	/// </summary>
	public void Lift()
	{
		RaycastHit hit;
		if (_pushing == false)
		{
			if (_lifting == false)
			{
				if (Physics.Raycast(_rayPos, _forward, out hit, _rayLength, _layerMask))
				{
					_interactedObject = hit.collider.GetComponent<Rigidbody>();
					_ioStartPosition = _interactedObject.GetComponent<Transform>().position;
					_interactedObject.transform.position = this.transform.position + new Vector3(0, 2.5f, 0);
					_currentState = STATE.LIFTING;
					_lifting = true;
				}
			}
			else if (_lifting == true)
			{
				_lifting = false;
				_interactedObject.GetComponent<Transform>().position = _ioStartPosition;
				_interactedObject = null;
				_ioStartPosition = Vector3.zero; 
				_currentState = STATE.FREE;
			}
		}
	}


}
