﻿using UnityEngine;

public class VelocityBasedMovement : IMovementController
{
    private float _speed;
    private Rigidbody _rb;

    public VelocityBasedMovement(Rigidbody rb, float speed)
    {
        _rb = rb;
        _speed = speed;
    }

    public void Move(Vector3 dir)
    {
        _rb.velocity = new Vector3(dir.x * _speed, _rb.velocity.y, dir.z * _speed);
    }

    public void AddExternalForce(Vector3 force)
    {
        throw new System.NotImplementedException();
    }

    public void AddForce(Vector3 force)
    {
        throw new System.NotImplementedException();
    }

    public void FixedUpdate()
    {
        //throw new System.NotImplementedException();
    }

    public void OnCollisionEnter(Collision collision)
    {
        
    }
}