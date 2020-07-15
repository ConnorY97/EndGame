using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10.0f;
    public float maxSpeed = 100.0f;
    private Rigidbody _rb;
    private bool _holding = false;

    Vector3 _mosuePos;
    //Transform _trans;
    Vector3 _objPos;
    float _angle; 

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
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
            if (Physics.Raycast(transform.position - new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.forward), out hit, 5, _layerMask))
            { 
                Debug.DrawRay(transform.position - new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                hit.collider.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity;
                _holding = true;
                _rb.velocity = -hit.normal * movement.z * speed; 
            }
            else
            {
                Debug.DrawRay(transform.position - new Vector3(0, 0.5f, 0), transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                Debug.Log("Did not Hit");
            }
        }

        


        //_rb.AddForce(movement * speed);

        //_rb.velocity = Vector3.ClampMagnitude(_rb.velocity, maxSpeed);

		if (!_holding)
            _rb.velocity = movement.normalized * speed; 
    }
}
