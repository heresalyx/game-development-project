using UnityEngine;

public class DoorLock : HackableObject
{
    // For every door, move door and disable identifier.
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
