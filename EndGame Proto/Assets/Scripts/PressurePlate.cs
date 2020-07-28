using UnityEngine;

public class PressurePlate : MonoBehaviour
{
	private bool _triggered = false;

	[SerializeField] private float _distance = 0;
	[SerializeField] private float _speed;

	private Vector3 _raisedPos;
	private Vector3 _depressedPos;
	private Vector3 _startPos; 

	private float _startTime;

	private Material _pressurePlateColour; 

	// Start is called before the first frame update
	void Start()
	{
		//Initialising positions of the pressure plate 
		_startPos = transform.position;
		_raisedPos = _startPos;
		_depressedPos = transform.position - new Vector3(0, _distance, 0);

		_pressurePlateColour = GetComponent<MeshRenderer>().material;
		_pressurePlateColour.color = Color.red; 
	}

	// Update is called once per frame
	void Update()
	{
		float elapsedTime = (Time.time - _startTime);
		float fractionOfJourney = elapsedTime / _speed; 

		//Moving the pressure plate to its two state
		//	raised and depressed depending on whether or not the Golem is standing on it 
		if (_triggered)
			transform.position = Vector3.Lerp(_startPos, _depressedPos, fractionOfJourney);
		else
			transform.position = Vector3.Lerp(_startPos, _raisedPos, fractionOfJourney);


	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.gameObject.tag.Equals("Orb"))
		{
			_triggered = true;
			_startTime = Time.time;

			_startPos = transform.position;
			_pressurePlateColour.color = Color.blue;
		} 
	}

	private void OnTriggerExit(Collider other)
	{
		if (!other.gameObject.tag.Equals("Orb"))
		{
			_triggered = false;
			_startTime = Time.time;

			_startPos = transform.position;

			_pressurePlateColour.color = Color.red;
		}
 
	}
}
