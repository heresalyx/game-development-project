using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowSecurityControlPanel : HackableObject
{
    public override void UnlockOutput()
    {
        outputGameObject.SetActive(true);
        objectCollider.enabled = false;
        gameObjectCanvas.enabled = false;
    }
}
