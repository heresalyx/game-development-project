using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogicGenerator : MonoBehaviour
{
    public PlayerController m_playerController;
    public AntiVirus m_antiVirus;
    public RectTransform m_logicParent;
    public Slider m_progressBar;
    public TextMeshProUGUI m_progressPercentage;
    public GameObject m_startPointPrefab;
    public GameObject m_endPointPrefab;
    public List<GameObject> m_logicGatePrefabs;
    private List<int> m_yPositionSteps = new List<int> {1, 2, 4, 8, 16};
    private List<float> m_yPositionStarts = new List<float> { 0.5f, 1, 2, 4, 8 };

    private GameObject m_endPoint;
    private LogicEndpoint m_endPointScript;
    private int m_currentLevel;
    private bool m_isComplete = false;
    private float m_progress = 0;

    // Set the properties of the new logic puzzle before creating the necessary parts.
    public void StartLogic(int level, int interupt, int difficulty)
    {
        m_currentLevel = level;
        StartCoroutine(CreateLogic());
        if (interupt != 0)
            StartCoroutine(Interuptor(interupt));
        if (difficulty != 0)
            m_antiVirus.Activate(difficulty);
    }

    // Create the end point of the circuit.
    public IEnumerator CreateLogic()
    {
        m_endPoint = Instantiate(m_endPointPrefab, m_logicParent);
        m_endPointScript = m_endPoint.GetComponent<LogicEndpoint>();
        m_endPointScript.SetParentNode(null);
        m_endPointScript.SetLogicGenerator(this);
        m_endPointScript.transform.localPosition = new Vector3((m_currentLevel * 250) / 2, 0, 0);

        // Call CreateRecursiveLogic Method with endPoint, level, and height = -1
        yield return new WaitUntil(() => CreateRecursiveLogic(m_endPointScript, m_currentLevel, -1) == true);

        while (m_endPointScript.IsOn())
        {
            yield return new WaitUntil(() => m_endPointScript.Shuffle() == true);
        }

        m_endPointScript.SetActive(true);
    }

    // Recurisivly creates the entire circuit.
    public bool CreateRecursiveLogic(LogicNode parentNode, int level, int height)
    {
        // If level == 1 then create startPoint at position height and return.
        if (level == 1)
        {
            GameObject startPoint = Instantiate(m_startPointPrefab, parentNode.gameObject.transform);
            LogicStartpoint startPointScript = startPoint.GetComponent<LogicStartpoint>();
            startPointScript.SetParentNode(parentNode);
            parentNode.AddInput(startPointScript);

            // Set Position to the left if height = -1 (first node)
            if (height == -1)
            {
                startPoint.transform.localPosition = new Vector3(-250, 0, 0);
                startPointScript.SetCircuit(-1, level);
            }
            else
            {
                startPoint.transform.localPosition = new Vector3(-250, (m_yPositionStarts[level - 1] - (m_yPositionSteps[level - 1] * height)) * 100, 0);
                startPointScript.SetCircuit(height, level);
            }
            return true;
        }

        // Create gate node at position height, then set parent as parentNode.
        GameObject currentPrefab = m_logicGatePrefabs[Random.Range(0, 2)];
        GameObject gateNode = Instantiate(currentPrefab, parentNode.gameObject.transform);
        LogicNode gateNodeScript = gateNode.GetComponent(currentPrefab.name) as LogicNode;
        gateNodeScript.SetParentNode(parentNode);
        parentNode.AddInput(gateNodeScript);

        // Set Position to the left if height = -1 (first node)
        if (height == -1)
        {
            gateNode.transform.localPosition = new Vector3(-250, 0, 0);
            gateNodeScript.SetCircuit(-1, level);
        }
        else
        {
            gateNode.transform.localPosition = new Vector3(-250, (m_yPositionStarts[level - 1] - (m_yPositionSteps[level - 1] * height)) * 100, 0);
            gateNodeScript.SetCircuit(height, level);
        }

        // For every i in level, call CreateRecursiveLogic Method with new gate node, level - 1, and height = i.
        for (int i = 0; i < 2; i++)
            CreateRecursiveLogic(gateNodeScript, level - 1, i);

        return true;
    }

    // Marks the logic as complete.
    public void SetLogicComplete()
    {
        m_isComplete = true;
    }

    // Distrupt and changes the logic depending on the severity given.
    public IEnumerator InteruptLogic(int severity)
    {
        // When severity is 0, there is no penalty.
        m_isComplete = false;
        // When severity is 1, the inputs are randomised.
        if (severity == 1)
        {
            StopCoroutine(m_antiVirus.CreateGlitch());
            StartCoroutine(m_antiVirus.CreateGlitch());
            m_endPointScript.SetActive(false);
            do
            {
                yield return new WaitUntil(() => m_endPointScript.Shuffle() == true);
            }
            while (m_endPointScript.IsOn());
            m_endPointScript.SetActive(true);
        }
        // When severity is 2, the entire circuit is changed.
        else if (severity == 2)
        {
            Destroy(m_endPoint);
            StartCoroutine(CreateLogic());
        }
    }

    // A loop randomising the inputs after a random amount of time.
    public IEnumerator Interuptor(int value)
    {
        yield return new WaitForSeconds(1 + ((Random.value * 30) / value));
        if (!m_antiVirus.IsSequence())
            StartCoroutine(InteruptLogic(1));
        StartCoroutine(Interuptor(value));
    }

    // Controls the progress made when the logic circuit is complete.
    public void FixedUpdate()
    {
        if (m_isComplete)
        {
            m_progress += 2f / Mathf.Pow(2, m_currentLevel);
            float falseProgress = Mathf.Log10((m_progress + (10 * ((100 - m_progress) / 100))) / 10) * 100;
            m_progressBar.value = falseProgress;
            m_progressPercentage.text = (int)falseProgress + "%";
            if (m_progress >= 100)
            {
                ExitLogic(0);
            }
        }
    }

    public void ExitLogic(int fatality)
    {
        if (fatality == 0)
        {
            m_antiVirus.StopPrompts();
            StopAllCoroutines();
            Destroy(m_endPoint);
            m_playerController.LogicComplete();
            m_isComplete = false;
            m_progress = 0;
            m_progressBar.value = 0;
            m_progressPercentage.text = "0%";
        }
        else if (fatality == 1)
        {
            m_antiVirus.StopPrompts();
            StopAllCoroutines();
            StartCoroutine(InteruptLogic(1));
        }
        else
        {
            m_antiVirus.StopPrompts();
            StopAllCoroutines();
            Destroy(m_endPoint);
            m_isComplete = false;
            m_progress = 0;
            m_progressBar.value = 0;
            m_progressPercentage.text = "0%";
            m_playerController.KillPlayer(true);
        }
    }
}
