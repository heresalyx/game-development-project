using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogicGenerator : MonoBehaviour
{
    public PlayerController playerController;
    public AntiVirus antiVirus;
    public RectTransform canvas;
    public Slider progressBar;
    public TextMeshProUGUI progressPercentage;
    public GameObject startPointPrefab;
    public GameObject endPointPrefab;
    public List<GameObject> LogicGatePrefabs;
    private List<int> steps = new List<int> {1, 2, 4, 8, 16};
    private List<float> yStart = new List<float> { 0.5f, 1, 2, 4, 8 };

    private GameObject endPoint;
    private LogicEndpoint endPointScript;
    private int currentLevel;
    private bool logicComplete = false;
    private float progress = 0;

    // Set the properties of the new logic puzzle before creating the necessary parts.
    public void StartLogic(int level, int interupt, int difficulty)
    {
        currentLevel = level;
        StartCoroutine(CreateLogic());
        if (interupt != 0)
            StartCoroutine(Interuptor(interupt));
        if (difficulty != 0)
            antiVirus.Activate(difficulty);
    }

    // Create the end point of the circuit.
    public IEnumerator CreateLogic()
    {
        if (canvas.childCount != 0)
            Destroy(canvas.GetChild(0).gameObject);
        endPoint = Instantiate(endPointPrefab, canvas);
        endPointScript = endPoint.GetComponent<LogicEndpoint>();
        endPointScript.SetParentNode(null);
        endPointScript.SetLogicGenerator(this);
        endPointScript.transform.localPosition = new Vector3((currentLevel * 250) / 2, 0, 0);

        // Call CreateRecursiveLogic Method with endPoint, level, and height = -1
        yield return new WaitUntil(() => CreateRecursiveLogic(endPointScript, currentLevel, -1) == true);

        while (endPointScript.IsOn())
        {
            yield return new WaitUntil(() => endPointScript.Shuffle() == true);
        }

        endPointScript.SetActive(true);
    }

    // Recurisivly creates the entire circuit.
    public bool CreateRecursiveLogic(LogicNode parentNode, int level, int height)
    {
        // If level == 1 then create startPoint at position height and return.
        if (level == 1)
        {
            GameObject startPoint = Instantiate(startPointPrefab, parentNode.gameObject.transform);
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
                startPoint.transform.localPosition = new Vector3(-250, (yStart[level - 1] - (steps[level - 1] * height)) * 100, 0);
                startPointScript.SetCircuit(height, level);
            }
            return true;
        }

        // Create gate node at position height, then set parent as parentNode.
        GameObject currentPrefab = LogicGatePrefabs[Random.Range(0, 2)];
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
            gateNode.transform.localPosition = new Vector3(-250, (yStart[level - 1] - (steps[level - 1] * height)) * 100, 0);
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
        logicComplete = true;
    }

    // Distrupt and changes the logic depending on the severity given.
    public IEnumerator InteruptLogic(int severity)
    {
        // When severity is 0, there is no penalty.
        logicComplete = false;
        // When severity is 1, the inputs are randomised.
        if (severity == 1)
        {
            StopCoroutine(antiVirus.CreateGlitch());
            StartCoroutine(antiVirus.CreateGlitch());
            endPointScript.SetActive(false);
            do
            {
                yield return new WaitUntil(() => endPointScript.Shuffle() == true);
            }
            while (endPointScript.IsOn());
            endPointScript.SetActive(true);
        }
        // When severity is 2, the entire circuit is changed.
        else if (severity == 2)
        {
            Destroy(endPoint);
            StartCoroutine(CreateLogic());
        }
    }

    // A loop randomising the inputs after a random amount of time.
    public IEnumerator Interuptor(int value)
    {
        yield return new WaitForSeconds(1 + ((Random.value * 30) / value));
        if (!antiVirus.IsSequence())
            StartCoroutine(InteruptLogic(1));
        StartCoroutine(Interuptor(value));
    }

    // Controls the progress made when the logic circuit is complete.
    public void FixedUpdate()
    {
        if (logicComplete)
        {
            progress += 2f / Mathf.Pow(2, currentLevel);
            float falseProgress = Mathf.Log10((progress + (10 * ((100 - progress) / 100))) / 10) * 100;
            progressBar.value = falseProgress;
            progressPercentage.text = (int)falseProgress + "%";
            if (progress >= 100)
            {
                antiVirus.StopPrompts();
                StopAllCoroutines();
                Destroy(endPoint);
                playerController.LogicComplete();
                logicComplete = false;
                progress = 0;
                progressBar.value = 0;
                progressPercentage.text = "0%";
            }
        }
    }
}
