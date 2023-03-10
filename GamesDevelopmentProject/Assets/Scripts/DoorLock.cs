using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : HackableObject
{
    public override void UnlockOutput()
    {
        foreach (GameObject output in outputGameObject)
        {
            output.transform.position = new Vector3(output.transform.position.x, output.transform.position.y + 2, output.transform.position.z);
            objectCollider.enabled = false;
            gameObjectCanvas.enabled = false;
        }
    }
}
