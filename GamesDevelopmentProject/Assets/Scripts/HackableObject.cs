using UnityEngine;

abstract public class HackableObject : MonoBehaviour
{
    protected Camera mainCamera;
    public Canvas gameObjectCanvas;
    public RectTransform identifier;
    public SphereCollider objectCollider;
    public GameObject[] outputGameObject;
    public int level;
    public int interupt;
    public int antiVirusDifficulty;

    // Set correct distance for identifier.
    private void Start()
    {
        mainCamera = Camera.main;
        gameObjectCanvas.worldCamera = mainCamera;
        gameObjectCanvas.planeDistance = 0.28f;
    }

    // Set position for the identifier.
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

    public int GetInterupt()
    {
        return interupt;
    }

    public int GetAntiVirusDifficulty()
    {
        return antiVirusDifficulty;
    }

    abstract public void UnlockOutput();
}
