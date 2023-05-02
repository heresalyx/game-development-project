using UnityEngine;
using UnityEngine.UI;

public class AntiVirusPrompt : MonoBehaviour
{
    public AudioClip m_promptLoadingEffect;
    public AudioSource m_effectSource;
    public Image m_promptSlider;
    public Image m_currentIcon;
    public Sprite m_upArrow;
    public Sprite m_leftArrow;
    public Sprite m_downArrow;
    public Sprite m_rightArrow;
    private int m_direction;
    private bool m_isResetting = false;

    // Display the correct direction for the player.
    public void SetDirection(int value)
    {
        m_direction = value;
        switch (value)
        {
            case 0:
                m_currentIcon.sprite = m_upArrow;
                break;
            case 1:
                m_currentIcon.sprite = m_leftArrow;
                break;
            case 2:
                m_currentIcon.sprite = m_downArrow;
                break;
            case 3:
                m_currentIcon.sprite = m_rightArrow;
                break;
        }
        m_promptSlider.fillAmount = 0;
    }

    //Stops resetting and changes the colour to green.
    public void SetCorrect()
    {
        m_isResetting = false;
        m_promptSlider.color = new Color(0, 1, 0);
        m_promptSlider.fillAmount = 100;
    }

    //Change the colour to red, allow resetting.
    public void SetIncorrect()
    {
        m_isResetting = true;
        m_promptSlider.color = new Color(1, 0, 0);
    }

    // Get slider.
    public Image GetSlider()
    {
        return m_promptSlider;
    }

    // Get direction.
    public int GetDirection()
    {
        return m_direction;
    }

    // One second animation showing unloading prompt.
    public void FixedUpdate()
    {
        if (m_isResetting)
        {
            m_promptSlider.fillAmount -= 0.02f;
            if (m_promptSlider.fillAmount <= 0)
                m_isResetting = false;
        }
    }

    // Play sound effect when prompt appears as quick-time event.
    public void PlayLoadingEffect()
    {
        m_effectSource.PlayOneShot(m_promptLoadingEffect);
    }
}
