using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowSecurityControlPanel : HackableObject
{
    public override void UnlockOutput()
    {
        foreach (GameObject output in outputGameObject)
        {
            output.GetComponent<SecurityCamera>().MakeInteractable();
            objectCollider.enabled = false;
            gameObjectCanvas.enabled = false;
        }
    }
}
