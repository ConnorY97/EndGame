using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public bool holding = false; 

	private void FixedUpdate()
	{
        int _layerMask = 1 << 8;
        holding = false; 
        RaycastHit hit;
        //      if (Physics.Raycast(transform.position + new Vector3(0, 2, 0), transform.TransformDirection(Vector3.forward), out hit, 5, layerMask))
        //{
        //          Debug.DrawRay(transform.position + new Vector3(0, 2, 0), transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        //          Debug.Log("Did hit"); 
        //}
        //      else
        //{
        //          Debug.DrawRay(transform.position + new Vector3(0, 2, 0), transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        //          Debug.Log("Did not Hit");
        //      }
        if (Input.GetKey(KeyCode.E))
		{
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5, _layerMask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                hit.collider.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity;
                holding = true; 
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                Debug.Log("Did not Hit");
            }
        }
    }
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
