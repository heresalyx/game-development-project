using System.Collections;
using UnityEngine;
using TMPro;

public class Underscorer : MonoBehaviour
{
    public TextMeshProUGUI m_text;
    private string m_originalText;

    private void Awake()
    {
        m_originalText = m_text.text;
    }

    void OnEnable()
    {
        StartCoroutine(UpdateText());
    }

    private IEnumerator UpdateText()
    {
        m_text.text = m_originalText + "_";
        yield return new WaitForSecondsRealtime(1.0f);
        m_text.text = m_originalText;
        yield return new WaitForSecondsRealtime(1.0f);
        StartCoroutine(UpdateText());
    }
}
