using UnityEngine;

public class VirtualCameraManager : MonoBehaviour
{
    private static VirtualCameraManager _instance;
    public static VirtualCameraManager instance => _instance;

    private const string _identifierTag = "VCam";

    // 1.) Limited to one active VCAM.
    [SerializeField] private GameObject _defaultVirtualCamera;
    private GameObject[] _virtualCameras;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Debug.LogWarning("Multiple VCAM managers present!");

        _virtualCameras = GameObject.FindGameObjectsWithTag(_identifierTag);
        ToggleVCam(_defaultVirtualCamera);
    }

    public void ToggleVCam(GameObject vcam)
    {
        vcam.SetActive(true);

        for (int i = 0; i < _virtualCameras.Length; i++)
        {
            if (_virtualCameras[i] != vcam)
                _virtualCameras[i].SetActive(false);
        }
    }
}