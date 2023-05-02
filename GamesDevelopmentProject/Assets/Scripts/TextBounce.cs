using UnityEngine;

public class TextBounce : MonoBehaviour
{
    public Transform m_textTransform;
    private int m_xMoveAmount = 250;
    private int m_yMoveAmount = 250;

    // Set starting position.
    private void Start()
    {
        m_textTransform.localPosition = new Vector2(Random.Range(-Screen.width/2, Screen.width/2), Random.Range(-Screen.height/2, Screen.height/2));
    }

    // Travel along a diagonal path, flipping the direction when the position exceeds the screen size.
    void Update()
    {
        m_textTransform.localPosition = new Vector2(m_textTransform.localPosition.x + (m_xMoveAmount * Time.unscaledDeltaTime), m_textTransform.localPosition.y + (m_yMoveAmount * Time.unscaledDeltaTime));
        if (m_textTransform.localPosition.x >= Screen.width / 2 || m_textTransform.localPosition.x <= -Screen.width / 2)
            FlipX();
        if (m_textTransform.localPosition.y >= Screen.height / 2 || m_textTransform.localPosition.y <= -Screen.height / 2)
            FlipY();
    }

    // Flip travel on the X axis.
    private void FlipX()
    {
        m_xMoveAmount *= -1;
    }

    // Flip travel on the Y axis.
    private void FlipY()
    {
        m_yMoveAmount *= -1;
    }
}
