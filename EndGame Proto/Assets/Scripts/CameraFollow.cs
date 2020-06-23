using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform target;
	public float smoothTime = 0.125f;
	public Vector3 offset;
	private Vector3 velocity = Vector3.zero;

	void Update()
	{
		Vector3 desiredPos = target.position + offset;
		Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothTime);
		transform.position = smoothPos;

		transform.LookAt(target);
	}
}
