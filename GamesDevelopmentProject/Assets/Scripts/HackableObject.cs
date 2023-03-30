using UnityEngine;

abstract public class HackableObject : MonoBehaviour
{
    protected Camera m_mainCamera;
    public Canvas m_gameObjectCanvas;
    public RectTransform m_identifier;
    public SphereCollider m_objectCollider;
    public GameObject[] m_outputGameObject;
    public int m_level;
    public int m_interupt;
    public int m_antiVirusDifficulty;

    public PlayerController controllerRemoveLater;

    // Set correct distance for identifier.
    private void Start()
    {
        m_mainCamera = Camera.main;
        m_gameObjectCanvas.worldCamera = m_mainCamera;
        m_gameObjectCanvas.planeDistance = 0.28f;
        controllerRemoveLater = GameObject.Find("ScriptObject").GetComponent<PlayerController>();
    }

    // Set position for the identifier.
    public virtual void Update()
    {
        Vector3 newPosition = m_mainCamera.WorldToScreenPoint(gameObject.transform.position);

        if (newPosition.z < 0)
            m_identifier.anchoredPosition = new Vector3(-300, -300, 0);
        else
            m_identifier.anchoredPosition = newPosition;
    }

    public int GetLevel()
    {
        return m_level;
    }

    public int GetInterupt()
    {
        return m_interupt;
    }

    public int GetAntiVirusDifficulty()
    {
        return m_antiVirusDifficulty;
    }

    abstract public void UnlockOutput();
}
