using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private bool _state = false;

    private void Start()
    {
        ToggleCursor(_state);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleCursor();
    }

    private void ToggleCursor()
    {
        Cursor.lockState = _state ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = _state;
        _state = !_state;
    }

    private void ToggleCursor(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = state;
        _state = state;
    }
}
