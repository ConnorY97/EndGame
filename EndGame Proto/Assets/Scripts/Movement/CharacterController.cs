using UnityEngine;

public class CharacterController : IMovementController
{
    // Settings
    private CharacterControllerSettings _settings;
    private float _maxSpeed;
    private float _timeToMaxSpeed;
    private float _friction;
    private float _gravity;
    private float _maxIncline;
    private LayerMask _groundLayer;

    // Cache
    private float _acceleration;
    private Vector3 _bufferedAcceleration;
    private Vector3 _bufferedDir;
    private bool _isGrounded;
    private Rigidbody _rb;

    public CharacterController(Rigidbody rb, CharacterControllerSettings settings)
    {
        _rb = rb;

        _settings = settings;
        _maxSpeed = settings.maxSpeed;
        _timeToMaxSpeed = settings.timeToMaxSpeed;
        _friction = settings.friction;
        _gravity = settings.gravity;
        _maxIncline = settings.maxIncline;
        _groundLayer = settings.groundLayer;

        _acceleration = _maxSpeed / _timeToMaxSpeed + _friction;
    }

    public void FixedUpdate()
    {
        Vector3 velocity = _rb.velocity;

        // FUCKING HATE THIS JANKY MATH.
        Vector3 acceleration = _bufferedAcceleration * Time.fixedDeltaTime;

        Debug.DrawRay(_rb.position + Vector3.up * 0.5f + _bufferedDir * 0.25f, Vector3.down * 0.5f, Color.magenta);
        RaycastHit hitInfo;

        //                                       Half radius seems to not raise character from ground
        if (Physics.Raycast(_rb.position + Vector3.up * 0.5f + _bufferedDir * 0.25f, Vector3.down, out hitInfo, 0.5f, _groundLayer))
        {
            Vector3 accelerationDir = Vector3.ProjectOnPlane(acceleration, hitInfo.normal).normalized;
            float accelerationMag = acceleration.magnitude;
            velocity += accelerationDir * accelerationMag; ;

            Vector3 velocityDir = Vector3.ProjectOnPlane(velocity, hitInfo.normal);
            float velocityMag = velocity.magnitude;
            velocity = velocityDir.normalized * velocityMag;

            Debug.DrawRay(_rb.position, velocityDir, Color.blue);
        }
        else
        {
            velocity += acceleration;
        }

        // add friction. INTERFERES WITH GRAVITY. FIX THIS!
        Vector3 postFrictionVelocity = velocity + -velocity.normalized * _settings.friction * Time.fixedDeltaTime;
        if (Vector3.Dot(velocity.normalized, postFrictionVelocity.normalized) > 0f)
            velocity += -velocity.normalized * _settings.friction * Time.fixedDeltaTime;
        else
            velocity = Vector3.zero;

        // gravity.
        Debug.Log(_isGrounded);
        if (!_isGrounded)
            velocity += Vector3.down * _gravity * Time.fixedDeltaTime;

        if (velocity.magnitude > _maxSpeed)
        {
            if (_isGrounded)
            {
                velocity = Vector3.ClampMagnitude(velocity, _maxSpeed);
            }
            else
            {
                // ensuring velocity is only clamped on the X & Z to prevent reducing a fall due to gravity.
                Vector3 velocityXZ = velocity;
                velocityXZ.y = 0f;

                velocityXZ = Vector3.ClampMagnitude(velocityXZ, _maxSpeed);
                velocity.x = velocityXZ.x;
                velocity.z = velocityXZ.z;
            }
        }

        _bufferedDir = Vector3.zero;
        _bufferedAcceleration = Vector3.zero;
        _rb.velocity = velocity;

        _isGrounded = false;
    }

    private ContactPoint[] _contacts = new ContactPoint[20];
    public void OnCollisionEnter(Collision collision)
    {
        collision.GetContacts(_contacts);

        for (int i = 0; i < _contacts.Length; i++)
        {
            if (_contacts[i].normal.y > 0f)
                _isGrounded = true;
        }
    }

    public void AddExternalForce(Vector3 force)
    {

    }

    public void AddForce(Vector3 force)
    {

    }

    public void Move(Vector3 dir)
    {
        _bufferedDir = dir;
        _bufferedAcceleration = dir * _acceleration;
    }
}

[CreateAssetMenu(fileName = "Character Controller Settings", menuName = "Settings/Character Controller", order = 0)]
public class CharacterControllerSettings : ScriptableObject
{
    public float maxSpeed;
    public float timeToMaxSpeed;
    public float friction;
    public float gravity;
    public float maxIncline;
    public LayerMask groundLayer;
}