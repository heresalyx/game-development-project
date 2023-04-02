using UnityEngine;

public class TextBounce : MonoBehaviour
{
    public Transform m_textTransform;
    private int m_xMoveAmount = 250;
    private int m_yMoveAmount = 250;

    private void Start()
    {
        m_textTransform.localPosition = new Vector2(Random.Range(-Screen.width/2, Screen.width/2), Random.Range(-Screen.height/2, Screen.height/2));
    }

    // Update is called once per frame
    void Update()
    {
        m_textTransform.localPosition = new Vector2(m_textTransform.localPosition.x + (m_xMoveAmount * Time.unscaledDeltaTime), m_textTransform.localPosition.y + (m_yMoveAmount * Time.unscaledDeltaTime));
        if (m_textTransform.localPosition.x >= Screen.width / 2 || m_textTransform.localPosition.x <= -Screen.width / 2)
            FlipX();
        if (m_textTransform.localPosition.y >= Screen.height / 2 || m_textTransform.localPosition.y <= -Screen.height / 2)
            FlipY();
    }

    private void FlipX()
    {
        m_xMoveAmount *= -1;
    }

    private void FlipY()
    {
        m_yMoveAmount *= -1;
    }
}
