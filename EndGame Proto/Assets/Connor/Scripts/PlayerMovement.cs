using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
	[SerializeField] private float _speed = 0;
	[SerializeField] private float _angularSpeed = 0;
	[SerializeField] private float _rayLength = 0;
	[SerializeField] private float _offSetDist = 0; 

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

	private Rigidbody _heldObject;
	private Vector3 _heldObjectNormal;
	private Vector3 _heldOjectStartPosition; 

	private int _layerMask = 1 << 8;

	private Vector3 _rayPos; 




	// Start is called before the first frame update
	void Start()
	{
		_rb = GetComponent<Rigidbody>();
		_cameraTransform = Camera.main.transform;
	}

	// Update is called once per frame
	void Update()
	{
		_rayPos = transform.position - new Vector3(0, 0.5f, 0);

		float movementHorizontal = Input.GetAxis("Horizontal");
		float movementVertical = Input.GetAxis("Vertical");

		_input = new Vector3(movementHorizontal, 0.0f, movementVertical);

		_angle = _cameraTransform.rotation.eulerAngles.y;
		_forward = Quaternion.AngleAxis(_angle, Vector3.up) * Vector3.forward;
		_right = Vector3.Cross(Vector3.up, _forward);

		_heading = _input.normalized.x * _right + _input.normalized.z * _forward;

		RaycastHit hit;

		Debug.DrawRay(_rayPos, _forward * _rayLength, Color.white);
		//Pushing 
		if (Input.GetKeyDown(KeyCode.E))
		{
			if (_lifting == false)
			{
				if (_holding == false)
				{
					if (Physics.Raycast(_rayPos, _forward, out hit, _rayLength, _layerMask))
					{
						_heldObject = hit.collider.GetComponent<Rigidbody>();
						_heldObject.isKinematic = false;
						_heldObject.GetComponent<FixedJoint>().connectedBody = _rb;
						_heldObjectNormal = hit.normal;
						_currentState = STATE.PUSHING;
						//transform.position = _heldObject.GetComponent<Transform>().position + (_heldObjectNormal * _offSetDist); 
						_holding = true;
					}
				}
				else if (_holding == true)
				{
					_heldObject.GetComponent<FixedJoint>().connectedBody = null;
					_heldObject.isKinematic = true;
					//_sj.connectedBody = null; 
					_heldObject.transform.SetParent(null);
					_holding = false;
					_currentState = STATE.FREE;
				}
			}

		}

		//Lifting 
		if (Input.GetKeyDown(KeyCode.Q))
		{
			if (_holding == false)
			{
				if (_lifting == false)
				{
					if (Physics.Raycast(_rayPos, _forward, out hit, _rayLength, _layerMask))
					{
						_heldObject = hit.collider.GetComponent<Rigidbody>();
						_heldOjectStartPosition = _heldObject.GetComponent<Transform>().position;
						_heldObject.GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
						_currentState = STATE.LIFTING;
						_lifting = true;
					}
				}
				else if (_lifting == true)
				{
					_lifting = false;
					_heldObject.GetComponent<Transform>().position = _heldOjectStartPosition;
					_currentState = STATE.FREE;
				}
			}

		}

		switch (_currentState)
		{
			case STATE.FREE:
				{
					Quaternion targetRotation = Quaternion.LookRotation(_forward, Vector3.up);
					transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _angularSpeed * Time.fixedDeltaTime);
					_rb.constraints = RigidbodyConstraints.FreezeRotation;
					_rb.velocity = _heading * _speed;
					//_sj.spring = 0;
					break;
				}
			case STATE.PUSHING:
				{
					_rb.velocity = _input.z * -_heldObjectNormal * _speed;
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
}
