using UnityEngine;
using UnityEngine.UI;

public class AntiVirusPrompt : MonoBehaviour
{
    public Image promptSlider;
    public Image currentIcon;
    public Sprite upArrow;
    public Sprite leftArrow;
    public Sprite downArrow;
    public Sprite rightArrow;
    private int direction;
    private bool reset = false;

    // Display the correct direction for the player.
    public void SetDirection(int value)
    {
        direction = value;
        switch (value)
        {
            case 0:
                currentIcon.sprite = upArrow;
                break;
            case 1:
                currentIcon.sprite = leftArrow;
                break;
            case 2:
                currentIcon.sprite = downArrow;
                break;
            case 3:
                currentIcon.sprite = rightArrow;
                break;
        }
        promptSlider.fillAmount = 0;
    }

    public void SetCorrect()
    {
        reset = false;
        promptSlider.color = new Color(0, 1, 0);
        promptSlider.fillAmount = 100;
    }

    public void SetIncorrect()
    {
        reset = true;
        promptSlider.color = new Color(1, 0, 0);
    }

    public Image GetSlider()
    {
        return promptSlider;
    }

    public int GetDirection()
    {
        return direction;
    }

    // One second animation showing unloading prompt.
    public void FixedUpdate()
    {
        if (reset)
        {
            promptSlider.fillAmount -= 0.02f;
            if (promptSlider.fillAmount <= 0)
                reset = false;
        }
    }
}
