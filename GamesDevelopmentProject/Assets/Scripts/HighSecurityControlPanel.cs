using UnityEngine;

public class HighSecurityControlPanel : HackableObject
{
    private int m_securityState = 2;
    public RectTransform m_secondIdentifier;

    // Change update to handle two identifiers;
    public override void Update()
    {
        Vector3 newPosition = m_mainCamera.WorldToScreenPoint(gameObject.transform.position);

        if (m_securityState == 2)
        {
            if (newPosition.z < 0)
                m_secondIdentifier.anchoredPosition = new Vector3(-300, -300, 0);
            else
                m_secondIdentifier.anchoredPosition = newPosition;
        }

        if (m_securityState == 1)
        {
            if (newPosition.z < 0)
                m_identifier.anchoredPosition = new Vector3(-300, -300, 0);
            else
                m_identifier.anchoredPosition = newPosition;
        }
    }

    public int GetSecurityState()
    {
        return m_securityState;
    }

    // Decrease the security level.
    public override void UnlockOutput()
    {
        if (m_securityState == 2)
        {
            m_secondIdentifier.anchoredPosition = new Vector3(-300, -300, 0);
            m_securityState = 1;
        }
        else if (m_securityState == 1)
        {
            Debug.Log("You Win");
            m_objectCollider.enabled = false;
            m_gameObjectCanvas.enabled = false;
            controllerRemoveLater.NextLevel();
        }
    }
}
