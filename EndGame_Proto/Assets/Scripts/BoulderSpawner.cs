using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderSpawner : MonoBehaviour
{
    [SerializeField] private float _explosionForce = 0;
    [SerializeField] private float _cloneLifeSpan = 0;
    [SerializeField] private GameObject _boulder;

    private Rigidbody _clone;

    public int _cloneAmount = 0;

    private float _timer = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

		if (_timer > _cloneLifeSpan)
		{
			DestroyBoulder();
		}

		if (_cloneAmount == 0)
        {
            SpawnBoulder();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Boulder"))
        {
            other.attachedRigidbody.AddForce(other.transform.forward * _explosionForce, ForceMode.Force);
        }
    }

    private void SpawnBoulder()
    {
        _clone = Instantiate(_boulder).GetComponent<Rigidbody>();
        _clone.AddForce(_clone.transform.right * _explosionForce, ForceMode.Force);
        _cloneAmount++;
    }

    private void DestroyBoulder()
    {
        if (_clone != null)
            Destroy(_clone.gameObject);
        _cloneAmount--;
        _timer = 0;
    }
}
