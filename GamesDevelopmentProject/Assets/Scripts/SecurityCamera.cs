using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    public PlayerController playerController;
    public Camera gameObjectCamera;
    public AudioListener gameObjectAudioListener;
    public GameObject gameObjectCanvas;
    public RectTransform identifier;

    private void Update()
    {
        SecurityCamera currentSecurityCamera = playerController.GetCurrentSecurityCamera();
        if (currentSecurityCamera != this)
        {
            Vector3 newPosition = currentSecurityCamera.GetCamera().WorldToScreenPoint(gameObject.transform.position);

            if (newPosition.z < 0)
                identifier.anchoredPosition = new Vector3(-300, -300, 0);
            else
                identifier.anchoredPosition = newPosition;
        }
    }

    public void ToggleActivation(bool value)
    {
        gameObjectCamera.enabled = value;
        gameObjectAudioListener.enabled = value;
        gameObjectCanvas.SetActive(!value);
    }

    public Camera GetCamera()
    {
        return gameObjectCamera;
    }

    public GameObject GetCanvas()
    {
        return gameObjectCanvas;
    }
}
