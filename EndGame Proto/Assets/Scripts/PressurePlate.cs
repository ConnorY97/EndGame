using UnityEngine;

public class PressurePlate : MonoBehaviour
{
	enum PressurePlateType { ORB, GOLEM }

	[SerializeField] private PressurePlateType _type;
	[SerializeField] private float _distance;
	[SerializeField] private float _speed;

	private Vector3 _raisedPos;
	private Vector3 _depressedPos;
	private Vector3 _startPos; 
	private bool _triggered = false;

	private float _startTime;

	private string _targetTag;

	void Start()
	{
		// Initialising positions of the pressure plate.
		_startPos = transform.position;
		_raisedPos = _startPos;
		_depressedPos = transform.position - new Vector3(0, _distance, 0);

		if (_type == PressurePlateType.ORB)
			_targetTag = "Orb";
		else
			_targetTag = "Golem";
	}

	void Update()
	{
		float elapsedTime = (Time.time - _startTime);
		float fractionOfJourney = elapsedTime / _speed; 

		// Moving the pressure plate to its two state.
		// raised and depressed depending on whether or not the target is standing on it .
		if (_triggered)
			transform.position = Vector3.Lerp(_startPos, _depressedPos, fractionOfJourney);
		else
			transform.position = Vector3.Lerp(_startPos, _raisedPos, fractionOfJourney);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.gameObject.CompareTag(_targetTag))
		{
			_triggered = true;
			_startTime = Time.time;

			_startPos = transform.position;
		} 
	}

	private void OnTriggerExit(Collider other)
	{
		if (!other.gameObject.CompareTag(_targetTag))
		{
			_triggered = false;
			_startTime = Time.time;

			_startPos = transform.position;
		}
	}
}