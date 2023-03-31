using UnityEngine;

public class DoorLock : HackableObject
{
    // For every door, move door and disable identifier.
    public override void UnlockOutput()
    {
        foreach (GameObject output in m_outputGameObject)
        {
            StartCoroutine(output.GetComponent<DoorAnimator>().OpenDoor());
        }
        m_objectCollider.enabled = false;
        m_gameObjectCanvas.enabled = false;
        m_lightIndicator.color = new Color(0, 1, 0);
    }
}
