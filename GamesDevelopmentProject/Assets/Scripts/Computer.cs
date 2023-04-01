using UnityEngine;

public class Computer : HackableObject
{
    private PlayerController m_playerController;

    public override void UnlockOutput()
    {
        Debug.Log("Hacked");
        SecurityCamera webcam = m_outputGameObject[0].GetComponent<SecurityCamera>();
        webcam.MakeInteractable();
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
}
