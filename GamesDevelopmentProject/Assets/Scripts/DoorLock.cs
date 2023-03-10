using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : HackableObject
{
    public override void UnlockOutput()
    {
        outputGameObject.transform.position = new Vector3(outputGameObject.transform.position.x, outputGameObject.transform.position.y + 2, outputGameObject.transform.position.z);
        objectCollider.enabled = false;
        gameObjectCanvas.enabled = false;
    }
}
