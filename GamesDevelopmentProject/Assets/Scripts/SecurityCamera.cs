using UnityEngine;
using Cinemachine;
using Cinemachine.PostFX;
using UnityEngine.Rendering;

public class SecurityCamera : MonoBehaviour
{
    private Camera mainCamera;
    public CinemachineVirtualCamera cinemachineCamera;
    public CinemachineVolumeSettings cinemachineVolume;
    public GameObject gameObjectCamera;
    public SphereCollider gameObjectCollider;
    public Canvas gameObjectCanvas;
    public RectTransform identifier;

    public float startXRotation;
    public float startYRotation;

    private void Start()
    {
        mainCamera = Camera.main;
        gameObjectCanvas.worldCamera = mainCamera;
        gameObjectCanvas.planeDistance = 0.12f;
    }

    // Update identifier position.
    private void Update()
    {
        if (!cinemachineCamera.enabled)
        {
            Vector3 newPosition = mainCamera.WorldToScreenPoint(gameObject.transform.position);

            if (newPosition.z < 0)
                identifier.anchoredPosition = new Vector3(-300, -300, 0);
            else
                identifier.anchoredPosition = newPosition;
        }
    }

    // Toggle whether the camera is being used or not.
    public void ToggleActivation(bool value)
    {
        cinemachineCamera.enabled = value;
        gameObjectCamera.SetActive(!value);
        gameObjectCanvas.enabled = !value;
    }

    // Make it possible to use this camera.
    public void MakeInteractable()
    {
        gameObjectCollider.enabled = true;
        gameObjectCanvas.enabled = true;
    }

    public CinemachineVirtualCamera GetCinemachineCamera()
    {
        return cinemachineCamera;
    }

    public float GetStartYRotation()
    {
        return startYRotation;
    }

    public float GetStartXRotation()
    {
        return startXRotation;
    }

    public void SetCinemachineProfile(VolumeProfile profile)
    {
        cinemachineVolume.m_Profile = profile;
    }
}
