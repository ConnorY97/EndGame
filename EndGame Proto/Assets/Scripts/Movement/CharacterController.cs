using UnityEngine;

public class CharacterController : IMovementController
{
    // Configurables
    private float _maxSpeed;
    private float _acceleration;
    private float _deceleration;
    private float _friction;
    private float _gravity;

    private float _maxIncline;

    private Vector3 _bufferedAcceleration;

    public void FixedUpdate()
    {
        Vector3 velocity;

        // add acceleration.
        Vector3 force = _bufferedAcceleration;
        _bufferedAcceleration = Vector3.zero;

        // force -= 

        // add deceleration.
        // add friction.

    }

    public void AddExternalForce(Vector3 force)
    {
        
    }

    public void AddForce(Vector3 force)
    {
        
    }

    public void Move(Vector3 dir)
    {
        _bufferedAcceleration = dir * _acceleration;
    }
}