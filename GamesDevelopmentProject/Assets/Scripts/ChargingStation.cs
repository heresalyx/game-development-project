using System.Collections;
using UnityEngine;

public class ChargingStation : HackableObject
{
    public bool m_hasComputer;
    private PlayerController m_playerController;

    public IEnumerator BlinkIndicator()
    {
        m_lightIndicator.enabled = !m_lightIndicator.enabled;
        yield return new WaitForSeconds(1f);
        StartCoroutine(BlinkIndicator());
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Robot"))
        {
            StartCoroutine(BlinkIndicator());
            m_physicalIdentifier.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Robot"))
        {
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
        SecurityCamera temp = m_outputGameObject[0].GetComponent<SecurityCamera>();
        m_playerController.SetSecurityCamera(temp);
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
