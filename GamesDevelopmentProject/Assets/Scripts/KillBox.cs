using UnityEngine;

public class KillBox : MonoBehaviour
{
    public SecurityGuard m_securityGuard;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Robot") && other.gameObject.name == "ActiveRobot")
        {
            Debug.Log("Detected Robot Trigger");
            m_securityGuard.KilledPlayer();
        }
    }
}
