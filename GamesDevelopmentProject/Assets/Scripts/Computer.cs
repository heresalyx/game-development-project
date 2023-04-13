using UnityEngine;

public class Computer : HackableObject
{
    public string m_textFileName;
    private PlayerController m_playerController;

    public override void UnlockOutput()
    {
        Debug.Log("Hacked");
        SecurityCamera webcam = m_outputGameObject[0].GetComponent<SecurityCamera>();
        webcam.MakeInteractable();
        webcam.SetCinemachineProfile(null);
        m_playerController.SetSecurityCamera(webcam);
    }

    public void SetPlayerController(PlayerController controller)
    {
        m_playerController = controller;
    }

    public override void SetIdentifierType(bool isPhysical)
    {
        m_digitalIdentifier.gameObject.SetActive(true);
    }

    public string GetTextFileName()
    {
        return m_textFileName;
    }
}
