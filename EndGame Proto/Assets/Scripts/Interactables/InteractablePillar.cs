using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ORIENTATION
{
    NORTH,
    EAST,
    SOUTH,
    WEST
}
public class InteractablePillar : MonoBehaviour, IInteractable
{
    private ORIENTATION _currentOrientation; 
    [SerializeField] private float _speed = 0; 

    // Start is called before the first frame update
    void Start()
    {
        _currentOrientation = ORIENTATION.NORTH; 

    }

    // Update is called once per frame
    void Update()
    {
        Quaternion targetRotation = Quaternion.AngleAxis((float)_currentOrientation * 90, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _speed * Time.deltaTime);  
    }

    public void Interact() 
	{
        if (_currentOrientation == ORIENTATION.WEST)
            _currentOrientation = ORIENTATION.NORTH;
        else
            ++_currentOrientation;
    }
}

