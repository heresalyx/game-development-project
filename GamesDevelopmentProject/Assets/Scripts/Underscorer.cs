using System.Collections;
using UnityEngine;
using TMPro;

public class Underscorer : MonoBehaviour
{
    public TextMeshProUGUI m_text;
    private string m_originalText;

    // Save the original text.
    private void Awake()
    {
        m_originalText = m_text.text;
    }

    // Start blinking when the GameObject is enabled.
    void OnEnable()
    {
        StartCoroutine(UpdateText());
    }

    // Alternate between adding and removing the underscore.
    private IEnumerator UpdateText()
    {
        m_text.text = m_originalText + "_";
        yield return new WaitForSecondsRealtime(1.0f);
        m_text.text = m_originalText;
        yield return new WaitForSecondsRealtime(1.0f);
        StartCoroutine(UpdateText());
    }
}
