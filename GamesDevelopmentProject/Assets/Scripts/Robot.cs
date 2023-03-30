using UnityEngine;
using Cinemachine;
using Cinemachine.PostFX;
using UnityEngine.Rendering;

public class Robot : HackableObject
{
    private PlayerController m_playerController;
    public CharacterController m_characterController;
    public CinemachineVolumeSettings m_cinemachineVolume;
    public Transform m_robotHead;

    // Enable its own camera on unlock.
    public override void UnlockOutput()
    {
        foreach (GameObject output in m_outputGameObject)
        {
            output.GetComponent<CinemachineVirtualCamera>().enabled = true;
            m_gameObjectCanvas.enabled = false;
        }
    }

    public CharacterController GetCharacterController()
    {
        return m_characterController;
    }

    public Transform GetRobotHead()
    {
        return m_robotHead;
    }

    public void SetPlayerController(PlayerController controller)
    {
        m_playerController = controller;
    }

    public void SetCinemachineProfile(VolumeProfile profile)
    {
        m_cinemachineVolume.m_Profile = profile;
    }

    // When entering the charging station, exit the robot view.
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("ChargingStation") && m_gameObjectCanvas.enabled == false)
        {
            foreach (GameObject output in m_outputGameObject)
            {
                output.GetComponent<CinemachineVirtualCamera>().enabled = false;
                m_gameObjectCanvas.enabled = true;
                m_playerController.ExitRobot();
            }
        }
    }
}
