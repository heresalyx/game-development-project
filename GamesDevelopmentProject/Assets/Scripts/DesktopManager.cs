using System.Collections;
using TMPro;
using UnityEngine;

public class DesktopManager : MonoBehaviour
{
    public PlayerController m_playerController;
    public TextMeshProUGUI m_textFileName;
    public TextMeshProUGUI m_timeText;
    public GameObject m_textFile;
    public TMP_InputField m_textFileContentBox;
    private string m_currentFileName;

    // Display the desktop menu with a text file path.
    public void DisplayDesktop(string fileName)
    {
        m_currentFileName = fileName;
        m_textFileName.text = fileName + ".txt";
        gameObject.SetActive(true);
        StartCoroutine(UpdateTime());
    }

    // Change the time to match real-time.
    public IEnumerator UpdateTime()
    {
        m_timeText.text = System.DateTime.Now.ToShortTimeString();
        yield return new WaitForSeconds(60);
        StartCoroutine(UpdateTime());
    }

    // Hide the desktop menu.
    public void HideDesktop()
    {
        m_currentFileName = null;
        Cursor.lockState = CursorLockMode.Locked;
        m_playerController.SetHackedObject(null);
        gameObject.SetActive(false);
        StopAllCoroutines();
    }

    // Open, read, and display the text file.
    public void OpenTextFile()
    {
        TextAsset textFile = Resources.Load<TextAsset>(m_currentFileName);
        m_textFile.SetActive(true);
        m_textFileContentBox.text = textFile.text;
    }

    // Close the text file.
    public void CloseTextFile()
    {
        m_textFileContentBox.text = "";
        m_textFile.SetActive(false);
    }
}
