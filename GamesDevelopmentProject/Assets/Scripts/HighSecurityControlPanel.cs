using UnityEngine;

public class HighSecurityControlPanel : HackableObject
{
    private int securityState = 2;
    public RectTransform secondIdentifier;

    // Change update to handle two identifiers;
    public override void Update()
    {
        Vector3 newPosition = mainCamera.WorldToScreenPoint(gameObject.transform.position);

        if (securityState == 2)
        {
            if (newPosition.z < 0)
                secondIdentifier.anchoredPosition = new Vector3(-300, -300, 0);
            else
                secondIdentifier.anchoredPosition = newPosition;
        }

        if (securityState == 1)
        {
            if (newPosition.z < 0)
                identifier.anchoredPosition = new Vector3(-300, -300, 0);
            else
                identifier.anchoredPosition = newPosition;
        }
    }

    public int GetSecurityState()
    {
        return securityState;
    }

    // Decrease the security level.
    public override void UnlockOutput()
    {
        if (securityState == 2)
        {
            secondIdentifier.anchoredPosition = new Vector3(-300, -300, 0);
            securityState = 1;
        }
        else if (securityState == 1)
        {
            Debug.Log("You Win");
            objectCollider.enabled = false;
            gameObjectCanvas.enabled = false;
        }
    }
}
