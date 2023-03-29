using UnityEngine;
using Cinemachine;
using Cinemachine.PostFX;
using UnityEngine.Rendering;

public class Robot : HackableObject
{
    private PlayerController playerController;
    public CharacterController characterController;
    public CinemachineVolumeSettings cinemachineVolume;
    public Transform robotHead;

    // Enable its own camera on unlock.
    public override void UnlockOutput()
    {
        foreach (GameObject output in outputGameObject)
        {
            output.GetComponent<CinemachineVirtualCamera>().enabled = true;
            gameObjectCanvas.enabled = false;
        }
    }

    public CharacterController GetCharacterController()
    {
        return characterController;
    }

    public Transform GetRobotHead()
    {
        return robotHead;
    }

    public void SetPlayerController(PlayerController controller)
    {
        playerController = controller;
    }

    public void SetCinemachineProfile(VolumeProfile profile)
    {
        cinemachineVolume.m_Profile = profile;
    }

    // When entering the charging station, exit the robot view.
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("ChargingStation") && gameObjectCanvas.enabled == false)
        {
            foreach (GameObject output in outputGameObject)
            {
                output.GetComponent<CinemachineVirtualCamera>().enabled = false;
                gameObjectCanvas.enabled = true;
                playerController.ExitRobot();
            }
        }
    }
}
