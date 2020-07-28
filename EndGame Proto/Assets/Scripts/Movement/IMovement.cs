using UnityEngine;

public interface IMovement
{
    void Move(Vector3 dir);
    void AddForce(Vector3 force);
    void AddExternalForce(Vector3 force);
}