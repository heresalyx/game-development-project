using System.Collections;
using UnityEngine;

public class ChargingStation : HackableObject
{
    public bool m_hasComputer;
    private PlayerController m_playerController;
    private bool m_isBlicking;

    public IEnumerator BlinkIndicator()
    {
        m_lightIndicator.enabled = !m_lightIndicator.enabled;
        yield return new WaitForSeconds(1f);
        StartCoroutine(BlinkIndicator());
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Robot") && !m_isBlicking)
        {
            m_isBlicking = true;
            StartCoroutine(BlinkIndicator());
            m_physicalIdentifier.gameObject.SetActive(false);
        }
    }

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

    public bool HasComputer()
    {
        return m_hasComputer;
    }

    public override void UnlockOutput()
    {
        Debug.Log("Hacked");
        SecurityCamera webcam = m_outputGameObject[0].GetComponent<SecurityCamera>();
        webcam.MakeInteractable();
        m_playerController.SetSecurityCamera(webcam);
    }

    public override void SetIdentifierType(bool isPhysical)
    {
        m_physicalIdentifier.gameObject.SetActive(true);
    }

    public void SetPlayerController(PlayerController controller)
    {
        m_playerController = controller;
    }
}
