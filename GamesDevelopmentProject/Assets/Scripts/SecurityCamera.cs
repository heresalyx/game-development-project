using UnityEngine;
using Cinemachine;
using Cinemachine.PostFX;
using UnityEngine.Rendering;

public class SecurityCamera : MonoBehaviour
{
    private Camera m_mainCamera;
    public CinemachineVirtualCamera m_cinemachineCamera;
    public CinemachineVolumeSettings m_cinemachineVolume;
    public GameObject m_gameObjectCamera;
    public SphereCollider m_gameObjectCollider;
    public Canvas m_gameObjectCanvas;
    public RectTransform m_identifier;

    public float m_startXRotation;
    public float m_startYRotation;

    private void Start()
    {
        m_mainCamera = Camera.main;
        m_gameObjectCanvas.worldCamera = m_mainCamera;
        m_gameObjectCanvas.planeDistance = 0.12f;
    }

    // Update identifier position.
    private void Update()
    {
        if (!m_cinemachineCamera.enabled)
        {
            Vector3 newPosition = m_mainCamera.WorldToScreenPoint(gameObject.transform.position);

            if (newPosition.z < 0)
                m_identifier.anchoredPosition = new Vector3(-300, -300, 0);
            else
                m_identifier.anchoredPosition = newPosition;
        }
    }

    // Toggle whether the camera is being used or not.
    public void ToggleActivation(bool value)
    {
        m_cinemachineCamera.enabled = value;
        m_gameObjectCamera.SetActive(!value);
        m_gameObjectCanvas.enabled = !value;
    }

    // Make it possible to use this camera.
    public void MakeInteractable()
    {
        m_gameObjectCollider.enabled = true;
        m_gameObjectCanvas.enabled = true;
    }

    public CinemachineVirtualCamera GetCinemachineCamera()
    {
        return m_cinemachineCamera;
    }

    public float GetStartYRotation()
    {
        return m_startYRotation;
    }

    public float GetStartXRotation()
    {
        return m_startXRotation;
    }

    public void SetCinemachineProfile(VolumeProfile profile)
    {
        m_cinemachineVolume.m_Profile = profile;
    }
}
