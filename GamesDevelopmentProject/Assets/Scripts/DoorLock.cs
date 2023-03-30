using UnityEngine;

public class DoorLock : HackableObject
{
    // For every door, move door and disable identifier.
    public override void UnlockOutput()
    {
        foreach (GameObject output in m_outputGameObject)
        {
            output.transform.position = new Vector3(output.transform.position.x, output.transform.position.y + 2, output.transform.position.z);
            m_objectCollider.enabled = false;
            m_gameObjectCanvas.enabled = false;
        }
    }
}
