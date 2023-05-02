using System.Collections;
using UnityEngine;

public class ChargingStation : HackableObject
{
    public bool m_hasComputer;
    public string m_textFileName;
    private PlayerController m_playerController;
    private bool m_isBlicking;

    // Make the internal light blink.
    public IEnumerator BlinkIndicator()
    {
        m_lightIndicator.enabled = !m_lightIndicator.enabled;
        yield return new WaitForSeconds(1f);
        StartCoroutine(BlinkIndicator());
    }

    // Turn off indicator and start blinking light when robot returns.
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Robot") && !m_isBlicking)
        {
            m_isBlicking = true;
            StartCoroutine(BlinkIndicator());
            m_physicalIdentifier.gameObject.SetActive(false);
        }
    }

    // Turn on indicator and turn off light when robot leaves.
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Robot") && m_isBlicking)
        {
            m_isBlicking = false;
            StopAllCoroutines();
            m_lightIndicator.enabled = false;
            m_physicalIdentifier.gameObject.SetActive(true);
        }
    }

    // Return true if computer is attached.
    public bool HasComputer()
    {
        return m_hasComputer;
    }

    // Return text file name of the computer.
    public string GetTextFileName()
    {
        return m_textFileName;
    }

    // Activate and switch to the webcam.
    public override void UnlockOutput()
    {
        SecurityCamera webcam = m_outputGameObject[0].GetComponent<SecurityCamera>();
        webcam.MakeInteractable();
        webcam.SetCinemachineProfile(null);
        m_playerController.SetSecurityCamera(webcam);
    }

    // Set the visability of the identifier.
    public override void SetIdentifierType(bool isPhysical)
    {
        m_physicalIdentifier.gameObject.SetActive(true);
    }

    // Set the reference to the Player Controller.
    public void SetPlayerController(PlayerController controller)
    {
        m_playerController = controller;
    }
}
