using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighSecurityControlPanel : HackableObject
{
    public int securityState = 2;
    public RectTransform secondIdentifier;

    public override void Update()
    {
        if (securityState == 2)
        {
            Vector3 newPosition = mainCamera.WorldToScreenPoint(gameObject.transform.position);

            if (newPosition.z < 0)
                secondIdentifier.anchoredPosition = new Vector3(-300, -300, 0);
            else
                secondIdentifier.anchoredPosition = newPosition;
            identifier.anchoredPosition = new Vector3(-300, -300, 0);
        }

        if (securityState == 1)
        {
            Vector3 newPosition = mainCamera.WorldToScreenPoint(gameObject.transform.position);

            if (newPosition.z < 0)
                identifier.anchoredPosition = new Vector3(-300, -300, 0);
            else
                identifier.anchoredPosition = newPosition;
            secondIdentifier.anchoredPosition = new Vector3(-300, -300, 0);
        }
    }

    public int GetSecurityState()
    {
        return securityState;
    }

    public override void UnlockOutput()
    {
        if (securityState == 2)
        {
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
