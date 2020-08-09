using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    enum PressurePlateType { ORB, GOLEM }

    [SerializeField] private PressurePlateType _type;
    [SerializeField] private GameObject[] _interactions;

    private string _targetTag;

    private void Awake()
    {
        if (_type == PressurePlateType.ORB)
            _targetTag = "Orb";
        else
            _targetTag = "Golem";
    }

    private void ToggleInteractions()
    {
        for (int i = 0; i < _interactions.Length; i++)
        {
            _interactions[i].GetComponent<IInteractable>().Interact();
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(_targetTag))
        {
            ToggleInteractions();
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag(_targetTag))
        {
            ToggleInteractions();
        }
    }
}