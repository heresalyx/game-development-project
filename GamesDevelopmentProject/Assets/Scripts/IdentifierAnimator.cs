using UnityEngine;
using TMPro;
using UnityEngine.UI.Extensions;

public class IdentifierAnimator : MonoBehaviour
{
    public TextMeshProUGUI m_digitalIdentifier;
    public UICircle m_physicalIdentifier;
    public Transform m_dockingIdentifierArrow;
    public bool m_isPhysical;
    public bool m_isDock;
    private float m_timer = 100;

    // Animate the text or UI of the identifiers.
    void Update()
    {
        m_timer -= 50 * Time.unscaledDeltaTime;
        if (m_timer <= 0)
            m_timer = 100;

        if (m_isDock)
        {
            m_dockingIdentifierArrow.localPosition = new Vector3(m_dockingIdentifierArrow.localPosition.x, (100 - m_timer) / 2, m_dockingIdentifierArrow.localPosition.z);
        }
        else
        {
            if (m_isPhysical)
            {
                m_physicalIdentifier.Padding = (int)m_timer;
                if (m_timer > 50)
                    m_physicalIdentifier.Thickness = (100 - m_timer) / 2;
                else
                    m_physicalIdentifier.Thickness = m_timer / 2;
                m_physicalIdentifier.SetAllDirty();
            }
            else
            {
                if (m_timer > 75)
                    m_digitalIdentifier.text = "\n.\n ";
                else if (m_timer > 50)
                    m_digitalIdentifier.text = "\n•\n ";
                else if (m_timer > 25)
                    m_digitalIdentifier.text = "\no\n ";
                else
                    m_digitalIdentifier.text = "--\n|    |\n--";
            }
        }

    }
}
