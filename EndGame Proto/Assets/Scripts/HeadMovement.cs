using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMovement : MonoBehaviour
{
	public float speed = 10.0f;
	public float maxSpeed = 100.0f; 
	private Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>(); 
	}

	private void FixedUpdate()
	{
		float movementHorizontal = Input.GetAxis("Horizontal");
		float movementVertical = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(movementHorizontal, 0.0f, movementVertical);

		rb.AddForce(movement * speed);

		rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed); 

		Debug.Log(rb.velocity.x); 

		//if (Input.GetKey(KeyCode.D))
		//{
		//	transform.position += Vector3.right * speed * Time.deltaTime;
		//}
		//if (Input.GetKey(KeyCode.A))
		//{
		//	transform.position += Vector3.left * speed * Time.deltaTime;
		//}
		//if (Input.GetKey(KeyCode.W))
		//{
		//	transform.position += Vector3.forward * speed * Time.deltaTime;
		//}
		//if (Input.GetKey(KeyCode.S))
		//{
		//	transform.position += Vector3.back * speed * Time.deltaTime;
		//}
	}
}
