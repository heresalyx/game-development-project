using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Robot : HackableObject
{
    public CharacterController characterController;
    public Transform robotHead;

    public override void UnlockOutput()
    {
        foreach (GameObject output in outputGameObject)
        {
            output.GetComponent<CinemachineVirtualCamera>().enabled = true;
            objectCollider.enabled = false;
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
}
