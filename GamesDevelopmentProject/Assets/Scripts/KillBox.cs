using UnityEngine;

public class KillBox : MonoBehaviour
{
    public SecurityGuard m_securityGuard;

    // Signal to the security guard that the player is dead.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Robot") && other.gameObject.name == "ActiveRobot")
        {
            m_securityGuard.KilledPlayer();
        }
    }
}
