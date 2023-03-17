using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogicGenerator : MonoBehaviour
{
    public PlayerController playerController;
    public AntiVirus antiVirus;
    public GameObject canvas;
    public GameObject startPointPrefab;
    public GameObject endPointPrefab;
    public List<GameObject> LogicGatePrefabs;
    private List<int> steps = new List<int> {1, 2, 4, 8, 16};
    private List<float> yStart = new List<float> { 0.5f, 1, 2, 4, 8 };
    public GameObject endPoint;
    public LogicEndpoint endPointScript;

    public int currentLevel;
    public bool logicComplete = false;
    public float progress = 0;
    public Slider progressBar;
    public TextMeshProUGUI progressPercentage;

    public void StartLogic(int level, int interupt, int difficulty)
    {
        StartCoroutine(CreateLogic(level));
        if (interupt != 0)
            StartCoroutine(Interuptor(interupt));
        antiVirus.Activate(difficulty);
    }

    public IEnumerator CreateLogic(int level)
    {
        currentLevel = level;
        if (canvas.transform.childCount != 0)
            Destroy(canvas.transform.GetChild(0).gameObject);
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(1904, 12000);
        endPoint = Instantiate(endPointPrefab, canvas.transform);
        endPointScript = endPoint.GetComponent<LogicEndpoint>();
        endPointScript.parentNode = null;
        endPointScript.currentToggle = endPoint.GetComponent<Toggle>();
        endPointScript.SetLogicGenerator(this);
        endPointScript.transform.localPosition = new Vector3((level * 250) / 2, 0, 0);

        //Call CreateRecursiveLogic Method with endPoint, level, and height = -1
        yield return new WaitUntil(() => CreateRecursiveLogic(endPointScript, level, -1) == true);

        while (endPointScript.GetToggleActive())
        {
            yield return new WaitUntil(() => endPointScript.Shuffle() == true);
            Debug.Log("Shuffled");
        }

        endPointScript.SetActive(true);
    }

    public bool CreateRecursiveLogic(LogicNode parentNode, int level, int height)
    {
        //If level = 1 then
        //Create startPoint at position height and return
        Toggle toggle;

        if (level == 1)
        {
            GameObject startPoint = Instantiate(startPointPrefab, parentNode.gameObject.transform);
            LogicStartpoint startPointScript = startPoint.GetComponent<LogicStartpoint>();
            startPointScript.parentNode = parentNode;
            toggle = startPoint.GetComponent<Toggle>();
            startPointScript.SetRandomToggle(toggle);
            parentNode.AddInput(startPointScript);

            //Set Position to the left if height = -1 (first node)
            if (height == -1)
                startPoint.transform.localPosition = new Vector3(-250, 0, 0);
            else
            {
                //Debug.Log("Level = " + level + ", y = " + (yStart[level - 1] - steps[level - 1]) + "\nyStart = " + (yStart[level - 1]) + ", step = " + (steps[level - 1])); 
                startPoint.transform.localPosition = new Vector3(-250, (yStart[level - 1] - (steps[level - 1] * height)) * 100, 0);
            }
            return true;
        }

        //Create gate node at position height, then set parent as parentNode.
        GameObject currentPrefab;
        if (height == -1)
            currentPrefab = LogicGatePrefabs[Random.Range(0, 2)];
        else
            currentPrefab = LogicGatePrefabs[Random.Range(0, 2)];
        GameObject gateNode = Instantiate(currentPrefab, parentNode.gameObject.transform);
      

        //Debug.Log(currentPrefab.name);
        LogicNode gateNodeScript = gateNode.GetComponent(currentPrefab.name) as LogicNode;
        //Debug.Log(gateNodeScript);
        
        gateNodeScript.parentNode = parentNode;
        toggle = gateNode.GetComponent<Toggle>();
        gateNodeScript.currentToggle = toggle;
        parentNode.AddInput(gateNodeScript);

        //Set Position to the left if height = -1 (first node)
        if (height == -1)
            gateNode.transform.localPosition = new Vector3(-250, 0, 0);
        else
        {
            //Debug.Log("Level = " + level + ", y = " + (yStart[level - 1] - steps[level - 1]) + "\nyStart = " + (yStart[level - 1]) + ", step = " + (steps[level - 1]));
            gateNode.transform.localPosition = new Vector3(-250, (yStart[level - 1] - (steps[level - 1] * height)) * 100, 0);
        }

        //For every i in level
        //Call CreateRecursiveLogic Method with new gate node, level - 1, and height = i.

        for (int i = 0; i < 2; i++)
            CreateRecursiveLogic(gateNodeScript, level - 1, i);

        return true;
    }

    public void LogicComplete()
    {
        Debug.Log("Logic Complete");
        logicComplete = true;
    }

    public IEnumerator LogicInterupted(int severity)
    {
        Debug.Log("Logic Interupted");
        logicComplete = false;
        if (severity == 1)
        {
            endPointScript.SetActive(false);

            do
            {
                yield return new WaitUntil(() => endPointScript.Shuffle() == true);
                Debug.Log("Shuffled");
            }
            while (endPointScript.GetToggleActive());
               
            endPointScript.SetActive(true);
        }
        else if (severity == 2)
        {
            Destroy(endPoint);
            StartCoroutine(CreateLogic(currentLevel));
        }
    }

    public IEnumerator Interuptor(int value)
    {
        yield return new WaitForSecondsRealtime((Random.value * 30) / value);
        StartCoroutine(LogicInterupted(1));
        StartCoroutine(Interuptor(value));
    }

    public void FixedUpdate()
    {
        if (logicComplete)
        {
            progress += 2f / Mathf.Pow(2, currentLevel);
            progressBar.value = Mathf.Log10((progress + (10 * ((100 - progress)/ 100))) / 10) * 100;
            progressPercentage.text = (int)(Mathf.Log10((progress + (10 * ((100 - progress) / 100))) / 10) * 100) + "%";
            if (progress >= 100)
            {
                Debug.Log("Puzzle Complete");
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
