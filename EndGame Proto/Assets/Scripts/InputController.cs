using UnityEngine;

public class InputController : MonoBehaviour
{
    private IRequireInput _dest;
    private InputData _localInputData;

    private void Awake()
    {
        _dest = GetComponent<IRequireInput>();
    }

    private void Update()
    {
        _localInputData.input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _localInputData.normalisedInput = _localInputData.input.normalized;

        _dest.SetInputData(_localInputData);
    }
}