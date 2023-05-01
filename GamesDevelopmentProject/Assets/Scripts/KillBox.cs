using UnityEngine;

public class KillBox : MonoBehaviour
{
    public SecurityGuard m_securityGuard;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Detected Trigger Kill" + other.gameObject.name + ", " + other.gameObject.CompareTag("Robot"));

        if (other.gameObject.CompareTag("Robot") && other.gameObject.name == "ActiveRobot")
        {
            Debug.Log("Detected Robot Trigger");
            m_securityGuard.KilledPlayer();
        }
    }
}
