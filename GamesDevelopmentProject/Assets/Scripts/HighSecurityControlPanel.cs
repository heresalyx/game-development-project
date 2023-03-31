using UnityEngine;

public class HighSecurityControlPanel : HackableObject
{
    public override void Start()
    {
        base.Start();
        m_securityState = 2;
    }

    // Decrease the security level.
    public override void UnlockOutput()
    {
        if (m_securityState == 2)
        {
            m_securityState = 1;
            SetIdentifierType(!m_isPhysical);
        }
        else if (m_securityState == 1)
        {
            Debug.Log("You Win");
            m_objectCollider.enabled = false;
            m_gameObjectCanvas.enabled = false;
            m_lightIndicator.color = new Color(0, 1, 0);
        }
    }
}
