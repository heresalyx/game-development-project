using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class HackableObject : MonoBehaviour
{
    public GameObject[] outputGameObject;
    public int level;
    public Camera mainCamera;
    public Canvas gameObjectCanvas;
    public RectTransform identifier;
    public SphereCollider objectCollider;

    private void Start()
    {
        mainCamera = Camera.main;
        gameObjectCanvas.worldCamera = mainCamera;
        gameObjectCanvas.planeDistance = 0.28f;
    }

    public virtual void Update()
    {
        Vector3 newPosition = mainCamera.WorldToScreenPoint(gameObject.transform.position);

        if (newPosition.z < 0)
            identifier.anchoredPosition = new Vector3(-300, -300, 0);
        else
            identifier.anchoredPosition = newPosition;
    }

    public int GetLevel()
    {
        return level;
    }

    abstract public void UnlockOutput();
}
