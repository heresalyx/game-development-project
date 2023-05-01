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

    public void DisplayDesktop(string fileName)
    {
        m_currentFileName = fileName;
        m_textFileName.text = fileName + ".txt";
        gameObject.SetActive(true);
        StartCoroutine(UpdateTime());
    }

    public IEnumerator UpdateTime()
    {
        m_timeText.text = System.DateTime.Now.ToShortTimeString();
        yield return new WaitForSeconds(60);
        StartCoroutine(UpdateTime());
    }

    public void HideDesktop()
    {
        m_currentFileName = null;
        Cursor.lockState = CursorLockMode.Locked;
        m_playerController.SetHackedObject(null);
        gameObject.SetActive(false);
        StopAllCoroutines();
    }

    public void OpenTextFile()
    {
        TextAsset textFile = Resources.Load<TextAsset>(m_currentFileName);
        m_textFile.SetActive(true);
        m_textFileContentBox.text = textFile.text;
        //StreamReader streamReader = new StreamReader("Assets/Story/" + m_currentFileName + ".txt");
        //m_textFile.SetActive(true);
        //m_textFileContentBox.text = streamReader.ReadToEnd();
    }

    public void CloseTextFile()
    {
        m_textFileContentBox.text = "";
        m_textFile.SetActive(false);
    }
}
