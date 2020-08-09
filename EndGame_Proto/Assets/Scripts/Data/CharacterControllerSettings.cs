using UnityEngine;

[CreateAssetMenu(fileName = "Character Controller Settings", menuName = "Settings/Character Controller", order = 0)]
public class CharacterControllerSettings : ScriptableObject
{
    public float maxSpeed;
    public float timeToMaxSpeed;
    public float friction;
    public float gravity;
    public float maxIncline;
    public LayerMask groundLayer;

    public float colliderRadius;
    public float rayOffset;
}