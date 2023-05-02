using UnityEngine;

public class Computer : HackableObject
{
    public string m_textFileName;
    private PlayerController m_playerController;

    // Activate and switch to the webcam.
    public override void UnlockOutput()
    {
        SecurityCamera webcam = m_outputGameObject[0].GetComponent<SecurityCamera>();
        webcam.MakeInteractable();
        webcam.SetCinemachineProfile(null);
        m_playerController.SetSecurityCamera(webcam);
    }

    // Set the reference to the Player Controller.
    public void SetPlayerController(PlayerController controller)
    {
        m_playerController = controller;
    }

    // Set the visability of the identifier.
    public override void SetIdentifierType(bool isPhysical)
    {
        m_digitalIdentifier.gameObject.SetActive(true);
    }

    // Return text file name of the computer.
    public string GetTextFileName()
    {
        return m_textFileName;
    }
}
