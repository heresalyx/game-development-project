using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AntiVirus : MonoBehaviour
{
    public bool isSequence = false;
    public VolumeProfile hackingProfile;
    public GlitchVolume currentGlitch;
    public LogicGenerator logicGenerator;
    public Transform logicCanvas;
    public Transform sequenceCanvas;
    public Transform sequenceParent;
    public Slider timer;
    public TextMeshProUGUI timerText;
    public GameObject promptPrefab;
    public GameObject currentPrompt;
    public Image promptSlider = null;
    public List<Key> keys;
    public Queue<AntiVirusPrompt> sequence = new Queue<AntiVirusPrompt>();

    public int currentDifficulty;
    public float progress;

    // Start is called before the first frame update
    public void Activate(int difficulty)
    {
        currentDifficulty = difficulty;
        StartCoroutine(SendPrompt());
        hackingProfile.TryGet<GlitchVolume>(out currentGlitch);
    }

    public IEnumerator SendPrompt()
    {
        yield return new WaitForSecondsRealtime((Random.value * 50) / currentDifficulty);
        CreatePrompt();
        yield return new WaitForSecondsRealtime(5);
        StartCoroutine(SendPrompt());
    }

    public void CreatePrompt()
    {
        currentPrompt = Instantiate(promptPrefab, logicCanvas);
        AntiVirusPrompt currentPromptScript = currentPrompt.GetComponent<AntiVirusPrompt>();
        currentPromptScript.SetDirection(Random.Range(0, 4));
        promptSlider = currentPromptScript.GetSlider();
        sequence.Enqueue(currentPromptScript);
        Debug.Log("Created Prompt: " + sequence.Peek().GetDirection());
    }

    public void CreateSequence()
    {
        StopPrompts();
        StartCoroutine(logicGenerator.LogicInterupted(2));
        isSequence = true;
        sequenceCanvas.gameObject.SetActive(true);
        Debug.Log("Creating Sequence");
        sequenceParent = Instantiate(new GameObject(), sequenceCanvas).transform;
        for (int i = 0; i < currentDifficulty; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Debug.Log("Creating Prompt " + (j + 1) + " out of " + currentDifficulty);
                currentPrompt = Instantiate(promptPrefab, sequenceParent);
                currentPrompt.transform.localPosition = new Vector3(-500 + (250f * j), 0.0f - (250f * i), 1.0f);
                AntiVirusPrompt currentPromptScript = currentPrompt.GetComponent<AntiVirusPrompt>();
                currentPromptScript.SetDirection(Random.Range(0, 4));
                sequence.Enqueue(currentPromptScript);
            }
        }

        timer.maxValue = (8 / currentDifficulty) + 2;
        progress = (8 / currentDifficulty) + 2;

        foreach (AntiVirusPrompt prompt in sequence)
        {
            Debug.Log(prompt.GetDirection());
        }
    }

    void FixedUpdate()
    {
        if (promptSlider != null && !isSequence)
        {
            promptSlider.fillAmount += 0.005f * currentDifficulty;
        }
        else if (isSequence)
        {
            progress -= 0.02f;
            timer.value = Mathf.Log10((progress + ((timer.maxValue / 10) * ((timer.maxValue - progress) / timer.maxValue))) / (timer.maxValue / 10)) * timer.maxValue;
            timerText.text = System.String.Format("{0:F2}", progress);
            currentGlitch.Speed.value = ((timer.maxValue - progress) / timer.maxValue) * 5;
            currentGlitch.BlockDensity.value = (timer.maxValue - progress) / timer.maxValue;
            currentGlitch.LineDensity.value = (timer.maxValue - progress) / timer.maxValue;
        }
    }

    private void Update()
    {
        if (Keyboard.current[Key.Space].wasPressedThisFrame)
        {
            Debug.Log("Space Pressed!!");
        }

        if (sequence.Count != 0)
        {
            if (Keyboard.current[keys[sequence.Peek().GetDirection()]].wasPressedThisFrame || Keyboard.current[keys[sequence.Peek().GetDirection() + 4]].wasPressedThisFrame)
            {
                Debug.Log("Correct!");
                if (!isSequence)
                {
                    promptSlider = null;
                    Destroy(sequence.Dequeue().gameObject);
                }
                else
                {
                    sequence.Dequeue().SetCorrect();
                    progress += (float)1 / currentDifficulty;
                }
                if (sequence.Count == 0)
                {
                    Debug.Log("Sequence Complete");                    
                    isSequence = false;
                    progress = 10;
                    if (sequenceParent != null)
                    {
                        Destroy(sequenceParent.gameObject);
                        sequence.Clear();
                        currentGlitch.Active.value = false;
                        currentGlitch.Speed.value = 5;
                        currentGlitch.BlockDensity.value = 1;
                        currentGlitch.LineDensity.value = 1;

                    }
                    sequenceCanvas.gameObject.SetActive(false);
                    Activate(currentDifficulty);
                }
                else if (sequence.Count % 5 == 0)
                {
                    sequenceParent.localPosition = new Vector3(sequenceParent.localPosition.x, sequenceParent.localPosition.y + 250f, sequenceParent.localPosition.z);
                }
            }
            else if (!isSequence)
            {
                if (Keyboard.current.anyKey.wasPressedThisFrame || promptSlider.fillAmount >= 1)
                {
                    Debug.Log("Incorrect / Too Late!");
                    Destroy(sequence.Dequeue().gameObject);
                    promptSlider = null;
                    CreateSequence();
                    currentGlitch.Active.value = true;
                }
            }
            else
            {
                if (Keyboard.current.anyKey.wasPressedThisFrame || progress <= 0)
                {
                    Debug.Log("Incorrect / Too Late! Game Over!");
                    isSequence = false;
                    progress = 10;
                    Destroy(sequenceParent.gameObject);
                    sequence.Clear();
                    sequenceCanvas.gameObject.SetActive(false);
                    Application.Quit();
                }
            }
        }
    }

    public void StopPrompts()
    {
        StopAllCoroutines();
        if (currentPrompt != null && !isSequence)
        {
            sequence.Clear();
            Destroy(currentPrompt);
            promptSlider = null;
        }
    }

    public IEnumerator CreateGlitch()
    {
        if (!isSequence)
        {
            Debug.Log("Triggered Glitch");
            currentGlitch.Active.value = true;
            yield return new WaitForSecondsRealtime(0.5f);
            currentGlitch.Active.value = false;
        }
    }
}
