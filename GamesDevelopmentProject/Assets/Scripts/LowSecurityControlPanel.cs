using UnityEngine;

public class LowSecurityControlPanel : HackableObject
{
    // For every camera, make them interactable.
    public override void UnlockOutput()
    {
        foreach (GameObject output in m_outputGameObject)
        {
            output.GetComponent<SecurityCamera>().MakeInteractable();
            m_objectCollider.enabled = false;
            m_gameObjectCanvas.enabled = false;
        }
    }
}
