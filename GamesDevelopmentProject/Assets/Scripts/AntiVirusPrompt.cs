using System.Collections;
using System.Collections.Generic;
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
    public int direction;

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
        promptSlider.color = new Color(0, 255, 0);
        promptSlider.fillAmount = 100;
    }

    public Image GetSlider()
    {
        return promptSlider;
    }

    public int GetDirection()
    {
        return direction;
    }
}
