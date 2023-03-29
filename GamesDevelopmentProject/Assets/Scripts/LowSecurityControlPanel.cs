using UnityEngine;

public class LowSecurityControlPanel : HackableObject
{
    // For every camera, make them interactable.
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
