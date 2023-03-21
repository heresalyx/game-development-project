using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.PostFX;
using UnityEngine.Rendering;

public class Robot : HackableObject
{
    public PlayerController playerController;
    public CharacterController characterController;
    public CinemachineVolumeSettings cinemachineVolume;
    public Transform robotHead;

    public override void UnlockOutput()
    {
        foreach (GameObject output in outputGameObject)
        {
            output.GetComponent<CinemachineVirtualCamera>().enabled = true;
            //objectCollider.enabled = false;
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

    public void LinkController(PlayerController controller)
    {
        playerController = controller;
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Collision!!!");
        if (collision.gameObject.tag == "ChargingStation" && gameObjectCanvas.enabled == false)
        {
            Debug.Log("Hit Charging Station");
            foreach (GameObject output in outputGameObject)
            {
                output.GetComponent<CinemachineVirtualCamera>().enabled = false;
                //objectCollider.enabled = true;
                gameObjectCanvas.enabled = true;
                playerController.ActivateCameraView();
            }
        }
    }

    public void SetVolumeProfile(VolumeProfile profile)
    {
        cinemachineVolume.m_Profile = profile;
    }
}
