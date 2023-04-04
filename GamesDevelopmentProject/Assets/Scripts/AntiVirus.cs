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
    public List<AudioClip> m_virusGlitchEffects;
    public AudioSource m_effectSource;
    public PlayerController m_playerController;
    public LogicGenerator m_logicGenerator;
    public VolumeProfile m_hackingProfile;
    public Transform m_logicCanvas;
    public Transform m_sequenceCanvas;
    public Slider m_timer;
    public TextMeshProUGUI m_timerText;
    public GameObject m_promptPrefab;
    public List<Key> m_keys;
    private GlitchVolume m_currentGlitch;

    private GameObject m_currentPrompt;
    private Image m_promptSlider = null;
    private Queue<AntiVirusPrompt> m_sequence = new Queue<AntiVirusPrompt>();
    private Queue<AntiVirusPrompt> m_sequenceBuffer = new Queue<AntiVirusPrompt>();
    private Transform m_sequenceParent;
    private bool m_isSequence = false;
    private int m_currentDifficulty;
    private float m_progress;
    private bool m_isGlitching;
    private int m_promptsRemaining = 0;

    // Grab and save the glitch volume.
    public void Start()
    {
        m_hackingProfile.TryGet<GlitchVolume>(out m_currentGlitch);
    }

    // Activate is called to start the anti-virus attacks for the first time in the puzzle.
    public void Activate(int difficulty)
    {
        m_currentDifficulty = difficulty;
        StartCoroutine(SendPrompt());
    }

    // A loop sending quick-time events after a certain length of time.
    public IEnumerator SendPrompt()
    {
        yield return new WaitForSeconds(5 + ((Random.value * 50) / m_currentDifficulty));
        CreatePrompt();
        StartCoroutine(SendPrompt());
    }

    // Create a quick-time event for the player.
    public void CreatePrompt()
    {
        m_currentPrompt = Instantiate(m_promptPrefab, m_logicCanvas);
        AntiVirusPrompt currentPromptScript = m_currentPrompt.GetComponent<AntiVirusPrompt>();
        currentPromptScript.SetDirection(Random.Range(0, 4));
        m_promptSlider = currentPromptScript.GetSlider();
        m_sequence.Enqueue(currentPromptScript);
        currentPromptScript.PlayLoadingEffect();
    }

    // Create a sequence for the player to replicate.
    public void CreateSequence()
    {
        StopPrompts();
        StartCoroutine(m_logicGenerator.InteruptLogic(2));
        m_isSequence = true;
        m_sequenceCanvas.gameObject.SetActive(true);
        StartCoroutine(CreateGlitch());
        m_sequenceParent = Instantiate(new GameObject(), m_sequenceCanvas).transform;
        for (int i = 0; i < m_currentDifficulty; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                m_currentPrompt = Instantiate(m_promptPrefab, m_sequenceParent);
                m_currentPrompt.transform.localPosition = new Vector3(-500 + (250f * j), 0.0f - (250f * i), 1.0f);
                AntiVirusPrompt currentPromptScript = m_currentPrompt.GetComponent<AntiVirusPrompt>();
                currentPromptScript.SetDirection(Random.Range(0, 4));
                m_sequence.Enqueue(currentPromptScript);
            }
        }
        m_timer.maxValue = (8 / m_currentDifficulty) + 2;
        m_progress = (8 / m_currentDifficulty) + 2;
    }

    // Handle the timer counting down when sequence is generated.
    void FixedUpdate()
    {
        // Progress prompt timer.
        if (m_promptSlider != null && !m_isSequence)
            m_promptSlider.fillAmount += 0.005f * m_currentDifficulty;
        // Progress sequence timer only if glitching has stopped.
        else if (m_isSequence)
        {
            if (!m_isGlitching)
            {
                float progressPercentage = (m_timer.maxValue - m_progress) / m_timer.maxValue;
                m_progress -= 0.02f;
                m_timer.value = Mathf.Log10((m_progress + ((m_timer.maxValue / 10) * progressPercentage)) / (m_timer.maxValue / 10)) * m_timer.maxValue;
                m_timerText.text = string.Format("{0:F2}", m_progress);
                m_currentGlitch.Speed.value = progressPercentage * 5;
                m_currentGlitch.BlockDensity.value = progressPercentage;
                m_currentGlitch.LineDensity.value = progressPercentage;
                m_effectSource.volume = progressPercentage;
            }
        }
    }

    // Handle the input given by the user.
    private void Update()
    {
        if (m_sequence.Count != 0 && !m_isGlitching && m_promptsRemaining == 0 && !m_playerController.InMenu())
        {
            if (Keyboard.current[m_keys[m_sequence.Peek().GetDirection()]].wasPressedThisFrame || Keyboard.current[m_keys[m_sequence.Peek().GetDirection() + 4]].wasPressedThisFrame)
            {
                if (!m_isSequence)
                {
                    m_promptSlider = null;
                    Destroy(m_sequence.Dequeue().gameObject);
                }
                else
                {
                    AntiVirusPrompt temp = m_sequence.Dequeue();
                    temp.SetCorrect();
                    m_sequenceBuffer.Enqueue(temp);
                    m_progress += (float)1 / m_currentDifficulty;
                }
                if (m_sequence.Count == 0)
                {
                    m_isSequence = false;
                    m_progress = 10;
                    if (m_sequenceParent != null)
                    {
                        Destroy(m_sequenceParent.gameObject);
                        m_sequence.Clear();
                        m_sequenceBuffer.Clear();
                        m_currentGlitch.Active.value = false;
                        m_currentGlitch.Speed.value = 5;
                        m_currentGlitch.BlockDensity.value = 1;
                        m_currentGlitch.LineDensity.value = 1;
                        m_effectSource.Stop();
                        m_effectSource.volume = 1;
                        StartCoroutine(SendPrompt());
                    }
                    m_sequenceCanvas.gameObject.SetActive(false);
                }
                else if (m_sequence.Count % 5 == 0)
                {
                    m_sequenceParent.localPosition = new Vector3(m_sequenceParent.localPosition.x, m_sequenceParent.localPosition.y + 250f, m_sequenceParent.localPosition.z);
                    m_sequenceBuffer.Clear();
                }
            }
            else if (!m_isSequence)
            {
                if ((Keyboard.current.anyKey.wasPressedThisFrame && !Keyboard.current[Key.Escape].wasPressedThisFrame) || m_promptSlider.fillAmount >= 1)
                {
                    Destroy(m_sequence.Dequeue().gameObject);
                    m_promptSlider = null;
                    CreateSequence();
                }
            }
            else
            {
                if ((Keyboard.current.anyKey.wasPressedThisFrame && !Keyboard.current[Key.Escape].wasPressedThisFrame))
                {
                    m_promptsRemaining = m_sequence.Count;
                    foreach (AntiVirusPrompt prompt in m_sequenceBuffer)
                    {
                        prompt.SetIncorrect();
                        m_sequence.Enqueue(prompt);
                    }
                    m_sequenceBuffer.Clear();

                    for (int i = 0; i < m_promptsRemaining; i++)
                        m_sequence.Enqueue(m_sequence.Dequeue());
                    m_promptsRemaining = 0;
                }

                if (m_progress <= 0)
                {
                    m_isSequence = false;
                    m_progress = 10;
                    Destroy(m_sequenceParent.gameObject);
                    m_sequence.Clear();
                    m_sequenceBuffer.Clear();
                    m_currentGlitch.Active.value = false;
                    m_currentGlitch.Speed.value = 5;
                    m_currentGlitch.BlockDensity.value = 1;
                    m_currentGlitch.LineDensity.value = 1;
                    m_effectSource.Stop();
                    m_effectSource.volume = 1;
                    m_sequenceCanvas.gameObject.SetActive(false);
                    m_logicGenerator.ExitLogic(2);
                }
            }
        }
    }

    // Stops all corountines running and clears any existing prompts.
    public void StopPrompts()
    {
        StopAllCoroutines();
        if (m_currentPrompt != null && !m_isSequence)
        {
            m_sequence.Clear();
            Destroy(m_currentPrompt);
            m_promptSlider = null;
        }
    }

    // Creates a glitch effect which also disables input temporarily when transitioning to sequence.
    public IEnumerator CreateGlitch()
    {
        if (!m_isSequence)
        {
            m_effectSource.PlayOneShot(m_virusGlitchEffects[Random.Range(0, m_virusGlitchEffects.Count)]);
            m_currentGlitch.Active.value = true;
            yield return new WaitForSecondsRealtime(0.5f);
            m_currentGlitch.Active.value = false;
        }
        else if (m_isSequence)
        {
            m_isGlitching = true;
            m_effectSource.volume = 0.0f;
            m_currentGlitch.Speed.value = 15f;
            m_effectSource.Play();
            m_currentGlitch.Active.value = true;
            yield return new WaitForSecondsRealtime(0.5f);
            m_isGlitching = false;
        }
    }

    // Returns isSequence.
    public bool IsSequence()
    {
        return m_isSequence;
    }
}
