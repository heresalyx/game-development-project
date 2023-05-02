using System.Collections;
using UnityEngine;

public class HighSecurityControlPanel : HackableObject
{
    public Transform m_door;

    // Set two levels of security.
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
            StartCoroutine(OpenDoor());
        }
        else if (m_securityState == 1)
        {
            m_objectCollider.enabled = false;
            m_gameObjectCanvas.enabled = false;
            m_lightIndicator.color = new Color(0, 1, 0);
        }
    }

    // Animate the front panel opening.
    public IEnumerator OpenDoor()
    {
        float count = 90;
        while (count > 0)
        {
            m_door.Rotate(new Vector3(0, -1.8f, 0), Space.Self);
            count -= 1.8f;
            yield return new WaitForSeconds(0.02f);
        }
    }
}
