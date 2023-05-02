using System.Collections;
using UnityEngine;

public class DoorAnimator : MonoBehaviour
{
    public Transform m_door;

    // Animate the door rotating open.
    public IEnumerator OpenDoor()
    {
        float count = 90;
        while (count > 0)
        {
            m_door.Rotate(new Vector3(0, -1.8f, 0), Space.Self);
            count -= 1.8f;
            yield return new WaitForSeconds(0.02f);
        }
    }
}
