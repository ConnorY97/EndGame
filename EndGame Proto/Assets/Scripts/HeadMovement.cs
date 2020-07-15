using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
	HEAD,
	ARMS,
	LEGS,
	FULL
}

public class HeadMovement : MonoBehaviour
{
	public float speed = 10.0f;
	public float maxSpeed = 100.0f; 
	private Rigidbody rb;

	private State currentState;

	public GameObject arms;
	public GameObject legs;

	public SphereCollider headCollider;
	public BoxCollider bodyCollider;
	public CapsuleCollider armsCollider; 

	private void Start()
	{
		rb = GetComponent<Rigidbody>(); 
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
			currentState = State.HEAD;
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			currentState = State.ARMS;
			rb.transform.position = new Vector3(rb.transform.position.x, 0.5f, rb.transform.position.z); 
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			currentState = State.LEGS;
			rb.transform.position = new Vector3(rb.transform.position.x, 0.5f, rb.transform.position.z);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			rb.transform.position = new Vector3(rb.transform.position.x, 0.5f, rb.transform.position.z);
			currentState = State.FULL;
		}
	}

	private void FixedUpdate()
	{

		//Depending on the state of the body will decide which method of movement is used 
		switch (currentState)
		{
			case State.HEAD:
				arms.SetActive(false);
				legs.SetActive(false);
				bodyCollider.enabled = false;
				armsCollider.enabled = false;
				break;
			case State.ARMS:
				arms.SetActive(true);
				legs.SetActive(false);
				bodyCollider.enabled = false;
				armsCollider.enabled = true;
				break;
			case State.LEGS:
				arms.SetActive(false);
				legs.SetActive(true);
				bodyCollider.enabled = true;
				armsCollider.enabled = false;
				break;
			case State.FULL:
				arms.SetActive(true);
				legs.SetActive(true);
				bodyCollider.enabled = true;
				armsCollider.enabled = false;
				break;
			default:
				break;
		}

		float movementHorizontal = Input.GetAxis("Horizontal");
		float movementVertical = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(movementHorizontal, 0.0f, movementVertical);

		rb.AddForce(movement * speed);

		rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

		//Debug.Log(rb.velocity.x);
	}
}
