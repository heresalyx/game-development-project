using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeathScreenAnimator : MonoBehaviour
{
    public RawImage m_jumpscare;
    public TextMeshProUGUI m_deathTitle;
    private float m_progress = 5;
    private bool m_isAnimating = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_isAnimating)
        {
            if (m_progress > 0)
                m_progress -= 0.02f;
            m_jumpscare.color = new Color(1.0f, 1.0f, 1.0f, (m_progress * 20) / 100);
            m_deathTitle.color = new Color(1.0f, 0.0f, 0.0f, ((5 - m_progress) * 20) / 100);
            m_deathTitle.characterSpacing = (5 - m_progress) * 5;
        }
    }

    public void StartAnimator()
    {
        m_isAnimating = true;
    }

    public void StopAnimator()
    {
        m_isAnimating = false;
        m_progress = 5;
    }
}
