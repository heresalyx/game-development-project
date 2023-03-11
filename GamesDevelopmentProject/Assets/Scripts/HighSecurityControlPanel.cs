using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighSecurityControlPanel : HackableObject
{
    public bool isUnlocked = false;

    public override void Update()
    {
        if (isUnlocked)
        {
            Vector3 newPosition = mainCamera.WorldToScreenPoint(gameObject.transform.position);

            if (newPosition.z < 0)
                identifier.anchoredPosition = new Vector3(-300, -300, 0);
            else
                identifier.anchoredPosition = newPosition;
        }
    }


    public override void UnlockOutput()
    {
        foreach (GameObject output in outputGameObject)
        {
            Debug.Log("You Win");
            objectCollider.enabled = false;
            gameObjectCanvas.enabled = false;
        }
    }

    public void MakeInteractable()
    {
        objectCollider.enabled = true;
        gameObjectCanvas.enabled = true;
        isUnlocked = true;
    }
}
