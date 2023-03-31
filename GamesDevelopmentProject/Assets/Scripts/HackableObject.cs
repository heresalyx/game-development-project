using UnityEngine;

abstract public class HackableObject : MonoBehaviour
{
    protected Camera m_mainCamera;
    public Canvas m_gameObjectCanvas;
    public RectTransform m_digitalIdentifier;
    public RectTransform m_physicalIdentifier;
    public Light m_lightIndicator;
    public SphereCollider m_objectCollider;
    public GameObject[] m_outputGameObject;
    public int m_level;
    public int m_interupt;
    public int m_antiVirusDifficulty;
    public bool m_isPhysical;
    public bool m_isLevelEnd;
    protected int m_securityState = 1;

    // Set correct distance for identifier.
    public virtual void Start()
    {
        m_mainCamera = Camera.main;
        m_gameObjectCanvas.worldCamera = m_mainCamera;
        m_gameObjectCanvas.planeDistance = 0.28f;
        SetIdentifierType(m_isPhysical);
    }

    // Set position for the identifier.
    public virtual void Update()
    {
        Vector3 newPosition = m_mainCamera.WorldToScreenPoint(gameObject.transform.position);

        if (m_isPhysical)
        {
            if (newPosition.z < 0)
                m_physicalIdentifier.anchoredPosition = new Vector3(-300, -300, 0);
            else
                m_physicalIdentifier.anchoredPosition = newPosition;
        }
        else
        {
            if (newPosition.z < 0)
                m_digitalIdentifier.anchoredPosition = new Vector3(-300, -300, 0);
            else
                m_digitalIdentifier.anchoredPosition = newPosition;
        }
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

    public bool IsPhysical()
    {
        return m_isPhysical;
    }

    public bool IsLevelEnd()
    {
        return m_isLevelEnd;
    }

    public virtual void SetIdentifierType(bool isPhysical)
    {
        m_isPhysical = isPhysical;
        if (m_isPhysical)
        {
            m_digitalIdentifier.gameObject.SetActive(false);
            m_physicalIdentifier.gameObject.SetActive(true);
        }
        else
        {
            m_digitalIdentifier.gameObject.SetActive(true);
            m_physicalIdentifier.gameObject.SetActive(false);
        }
    }

    abstract public void UnlockOutput();

    public int GetSecurityState()
    {
        return m_securityState;
    }
}
