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
    public PlayerController playerController;
    public LogicGenerator logicGenerator;
    public VolumeProfile hackingProfile;
    public Transform logicCanvas;
    public Transform sequenceCanvas;
    public Slider timer;
    public TextMeshProUGUI timerText;
    public GameObject promptPrefab;
    public List<Key> keys;
    private GlitchVolume currentGlitch;

    private GameObject currentPrompt;
    private Image promptSlider = null;
    private Queue<AntiVirusPrompt> sequence = new Queue<AntiVirusPrompt>();
    private Queue<AntiVirusPrompt> sequenceBuffer = new Queue<AntiVirusPrompt>();
    private Transform sequenceParent;
    private bool isSequence = false;
    private int currentDifficulty;
    private float progress;
    private bool isGlitching;
    private int promptsRemaining = 0;

    // Grab and save the glitch volume.
    public void Start()
    {
        hackingProfile.TryGet<GlitchVolume>(out currentGlitch);
    }

    // Activate is called to start the anti-virus attacks for the first time in the puzzle.
    public void Activate(int difficulty)
    {
        currentDifficulty = difficulty;
        StartCoroutine(SendPrompt());
    }

    // A loop sending quick-time events after a certain length of time.
    public IEnumerator SendPrompt()
    {
        yield return new WaitForSeconds(5 + ((Random.value * 50) / currentDifficulty));
        CreatePrompt();
        StartCoroutine(SendPrompt());
    }

    // Create a quick-time event for the player.
    public void CreatePrompt()
    {
        currentPrompt = Instantiate(promptPrefab, logicCanvas);
        AntiVirusPrompt currentPromptScript = currentPrompt.GetComponent<AntiVirusPrompt>();
        currentPromptScript.SetDirection(Random.Range(0, 4));
        promptSlider = currentPromptScript.GetSlider();
        sequence.Enqueue(currentPromptScript);
    }

    // Create a sequence for the player to replicate.
    public void CreateSequence()
    {
        StopPrompts();
        StartCoroutine(logicGenerator.InteruptLogic(2));
        isSequence = true;
        sequenceCanvas.gameObject.SetActive(true);
        StartCoroutine(CreateGlitch());
        sequenceParent = Instantiate(new GameObject(), sequenceCanvas).transform;
        for (int i = 0; i < currentDifficulty; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                currentPrompt = Instantiate(promptPrefab, sequenceParent);
                currentPrompt.transform.localPosition = new Vector3(-500 + (250f * j), 0.0f - (250f * i), 1.0f);
                AntiVirusPrompt currentPromptScript = currentPrompt.GetComponent<AntiVirusPrompt>();
                currentPromptScript.SetDirection(Random.Range(0, 4));
                sequence.Enqueue(currentPromptScript);
            }
        }
        timer.maxValue = (8 / currentDifficulty) + 2;
        progress = (8 / currentDifficulty) + 2;
    }

    // Handle the timer counting down when sequence is generated.
    void FixedUpdate()
    {
        // Progress prompt timer.
        if (promptSlider != null && !isSequence)
        {
            promptSlider.fillAmount += 0.005f * currentDifficulty;
        }
        // Progress sequence timer only if glitching has stopped.
        else if (isSequence)
        {
            if (!isGlitching)
            {
                float progressPercentage = (timer.maxValue - progress) / timer.maxValue;
                progress -= 0.02f;
                timer.value = Mathf.Log10((progress + ((timer.maxValue / 10) * progressPercentage)) / (timer.maxValue / 10)) * timer.maxValue;
                timerText.text = string.Format("{0:F2}", progress);
                currentGlitch.Speed.value = progressPercentage * 5;
                currentGlitch.BlockDensity.value = progressPercentage;
                currentGlitch.LineDensity.value = progressPercentage;
            }
        }
    }

    // Handle the input given by the user.
    private void Update()
    {
        if (sequence.Count != 0 && !isGlitching && promptsRemaining == 0 && !playerController.InMenu())
        {
            if (Keyboard.current[keys[sequence.Peek().GetDirection()]].wasPressedThisFrame || Keyboard.current[keys[sequence.Peek().GetDirection() + 4]].wasPressedThisFrame)
            {
                if (!isSequence)
                {
                    promptSlider = null;
                    Destroy(sequence.Dequeue().gameObject);
                }
                else
                {
                    AntiVirusPrompt temp = sequence.Dequeue();
                    temp.SetCorrect();
                    sequenceBuffer.Enqueue(temp);
                    progress += (float)1 / currentDifficulty;
                }
                if (sequence.Count == 0)
                {
                    isSequence = false;
                    progress = 10;
                    if (sequenceParent != null)
                    {
                        Destroy(sequenceParent.gameObject);
                        sequence.Clear();
                        sequenceBuffer.Clear();
                        currentGlitch.Active.value = false;
                        currentGlitch.Speed.value = 5;
                        currentGlitch.BlockDensity.value = 1;
                        currentGlitch.LineDensity.value = 1;
                        StartCoroutine(SendPrompt());
                    }
                    sequenceCanvas.gameObject.SetActive(false);
                }
                else if (sequence.Count % 5 == 0)
                {
                    sequenceParent.localPosition = new Vector3(sequenceParent.localPosition.x, sequenceParent.localPosition.y + 250f, sequenceParent.localPosition.z);
                    sequenceBuffer.Clear();
                }
            }
            else if (!isSequence)
            {
                if ((Keyboard.current.anyKey.wasPressedThisFrame && !Keyboard.current[Key.Escape].wasPressedThisFrame) || promptSlider.fillAmount >= 1)
                {
                    Destroy(sequence.Dequeue().gameObject);
                    promptSlider = null;
                    CreateSequence();
                }
            }
            else
            {
                if ((Keyboard.current.anyKey.wasPressedThisFrame && !Keyboard.current[Key.Escape].wasPressedThisFrame))
                {
                    promptsRemaining = sequence.Count;
                    foreach (AntiVirusPrompt prompt in sequenceBuffer)
                    {
                        prompt.SetIncorrect();
                        sequence.Enqueue(prompt);
                    }
                    sequenceBuffer.Clear();

                    for (int i = 0; i < promptsRemaining; i++)
                        sequence.Enqueue(sequence.Dequeue());
                    promptsRemaining = 0;
                }

                if (progress <= 0)
                {
                    isSequence = false;
                    progress = 10;
                    Destroy(sequenceParent.gameObject);
                    sequence.Clear();
                    sequenceBuffer.Clear();
                    currentGlitch.Active.value = false;
                    currentGlitch.Speed.value = 5;
                    currentGlitch.BlockDensity.value = 1;
                    currentGlitch.LineDensity.value = 1;
                    sequenceCanvas.gameObject.SetActive(false);
                    Application.Quit();
                }
            }
        }
    }

    // Stops all corountines running and clears any existing prompts.
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

    // Creates a glitch effect which also disables input temporarily when transitioning to sequence.
    public IEnumerator CreateGlitch()
    {
        if (!isSequence)
        {
            currentGlitch.Active.value = true;
            yield return new WaitForSecondsRealtime(0.5f);
            currentGlitch.Active.value = false;
        }
        else if (isSequence)
        {
            isGlitching = true;
            currentGlitch.Speed.value = 15f;
            currentGlitch.Active.value = true;
            yield return new WaitForSecondsRealtime(0.5f);
            isGlitching = false;
        }
    }

    // Returns isSequence.
    public bool IsSequence()
    {
        return isSequence;
    }
}
