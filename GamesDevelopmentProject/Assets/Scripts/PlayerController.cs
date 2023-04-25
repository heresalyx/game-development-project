using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public LogicGenerator m_logicGenerator;
    public Camera m_mainCamera;
    public AudioManager m_audioManager;
    public DesktopManager m_desktopManager;
    public AudioClip m_robotEffect;
    public AudioClip m_cameraEffect;
    public AudioClip m_whiteNoiseEffect;
    public AudioClip m_buttonPressEffect;
    public AudioSource m_effectSource;

    public GameObject m_cameraCanvas;
    public GameObject m_cursor;
    public TextMeshProUGUI m_timeText;
    public TextMeshProUGUI m_nameText;
    public TextMeshProUGUI m_liveText;

    public GameObject m_logicCanvas;

    public GameObject m_startCanvas;
    public GameObject m_deathCanvas;
    public DeathScreenAnimator m_deathAnimator;
    public GameObject m_pauseCanvas;
    public GameObject m_pauseMenu;
    public GameObject m_tutorialMenu;

    public VolumeProfile m_cameraProfile;
    public VolumeProfile m_hackingProfile;
    public VolumeProfile m_menuProfile;
    public VolumeProfile m_robotProfile;
    private VCRVolume m_VCRVolume;

    private int m_levelIndex = 1;
    private bool m_inCamera = true;
    public SecurityCamera m_securityCamera;
    private HackableObject m_hackedObject;
    private Robot m_robot;
    private CharacterController m_robotController;
    private Transform m_robotHead;

    private static float m_moveSpeed = 25;
    private float m_cameraStartXRotation;
    private float m_cameraStartYRotation;
    private float m_cameraXRotation;
    private float m_cameraYRotation;
    private Vector2 m_moveInput;

    private bool m_inMenu = true;
    private bool m_isPlayingAudio = false;

    public void Awake()
    {
        StartCoroutine(UpdateTime());
        StartCoroutine(LoadScene(1));
    }

    // Setup movement for first security camera.
    public void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        m_cameraProfile.TryGet(out m_VCRVolume);
        Debug.Log(m_VCRVolume);
        SetSecurityCamera(GameObject.Find("StartingSecurityCamera").GetComponentInChildren<SecurityCamera>());
    }

    // Handle the security camera and robot movement.
    public void Update()
    {
        if (m_hackedObject == null && m_inMenu == false)
        {
            // For cameras.
            if (m_inCamera)
            {
                // Use the x and y axis on currentLook to control cursor.
                Vector3 newLook = new Vector3(Mouse.current.delta.ReadValue().x, Mouse.current.delta.ReadValue().y);
                if (m_cursor.transform.localPosition.x + newLook.x > Screen.width / 2 || m_cursor.transform.localPosition.x + newLook.x < -Screen.width / 2)
                    newLook.x = 0;
                if (m_cursor.transform.localPosition.y + newLook.y > Screen.height / 2 || m_cursor.transform.localPosition.y + newLook.y < -Screen.height / 2)
                    newLook.y = 0;
                m_cursor.transform.localPosition += newLook;

                // Use the x and y axis on currentMove to control camera x rotation, except for webcams.
                if (m_securityCamera.name != "Webcam")
                {
                    float yMove = m_moveInput[1] * Time.deltaTime * m_moveSpeed;
                    if ((yMove > 0.01 || yMove < -0.01))
                    {
                        if (m_cameraXRotation - yMove < 15 + m_cameraStartXRotation && m_cameraXRotation - yMove > -15 + m_cameraStartXRotation)
                            m_cameraXRotation -= yMove;
                    }

                    float xMove = m_moveInput[0] * Time.deltaTime * m_moveSpeed;
                    if (xMove > 0.01 || xMove < -0.01)
                    {
                        if (m_cameraYRotation + xMove < 30 + m_cameraStartYRotation && m_cameraYRotation + xMove > -30 + m_cameraStartYRotation)
                            m_cameraYRotation += xMove;
                    }
                }                
                m_securityCamera.gameObject.transform.localRotation = Quaternion.Euler((-m_cursor.transform.localPosition.y / Screen.height * 9) + m_cameraXRotation, (m_cursor.transform.localPosition.x / Screen.width * 16) + m_cameraYRotation, 0f);
            }
            // For robots.
            else
            {
                // Use the x and y axis on currentLook to control cursor.
                Vector3 newRobotLook = new Vector3(0, Mouse.current.delta.ReadValue().x / 10);
                m_robot.transform.Rotate(newRobotLook);

                Vector3 newHeadLook = new Vector3(-Mouse.current.delta.ReadValue().y / 10, 0);

                if (m_robotHead.localEulerAngles.x + newHeadLook.x > 50 && m_robotHead.localEulerAngles.x + newHeadLook.x < 310)
                    newHeadLook = new Vector3(0, 0);
                m_robotHead.transform.Rotate(newHeadLook);

                Vector3 move = new Vector3(m_moveInput[0], 0, m_moveInput[1]);
                m_robotController.Move(m_robot.transform.TransformDirection(move * Time.deltaTime * 2));
            }


            if (m_moveInput != Vector2.zero && !m_isPlayingAudio)
            {
                m_effectSource.Play();
                m_isPlayingAudio = true;
            }
            else if (m_moveInput == Vector2.zero && m_isPlayingAudio)
            {
                m_effectSource.Stop();
                m_isPlayingAudio = false;
            }
        }
    }

    // This method uses the wasd keys.
    public void Move(InputAction.CallbackContext value)
    {
        //Store the most recent Vector2 value from the input.
        m_moveInput = value.ReadValue<Vector2>();
    }

    // When the interact button is clicked, find out if there was something that was clicked on.
    public void Interact(InputAction.CallbackContext value)
    {
        if (value.started && m_hackedObject == null && m_inMenu == false)
        {
            Ray cameraRay = m_mainCamera.ScreenPointToRay(new Vector3(m_cursor.transform.localPosition.x + (Screen.width / 2), m_cursor.transform.localPosition.y + (Screen.height / 2), 0));
            ButtonPress();

            // For cameras.
            if (m_inCamera)
            {
                bool objectHit = Physics.Raycast(cameraRay, out RaycastHit hit);

                if (objectHit && hit.collider.CompareTag("SecurityCamera"))
                {
                    SetSecurityCamera(hit.collider.gameObject.GetComponent<SecurityCamera>());
                }

                if (objectHit && hit.collider.CompareTag("DoorLock"))
                {
                    DoorLock doorLock = hit.collider.gameObject.GetComponent<DoorLock>();
                    if (!doorLock.IsPhysical())
                    {
                        m_hackedObject = hit.collider.gameObject.GetComponent<DoorLock>();
                        LogicInit(m_hackedObject.GetLevel(), m_hackedObject.GetInterupt(), m_hackedObject.GetAntiVirusDifficulty());
                    }
                }

                if (objectHit && hit.collider.CompareTag("LowSecurityControlPanel"))
                {
                    LowSecurityControlPanel panel = hit.collider.gameObject.GetComponent<LowSecurityControlPanel>();
                    if (!panel.IsPhysical())
                    {
                        m_hackedObject = hit.collider.gameObject.GetComponent<LowSecurityControlPanel>();
                        LogicInit(m_hackedObject.GetLevel(), m_hackedObject.GetInterupt(), m_hackedObject.GetAntiVirusDifficulty());
                    }
                }

                if (objectHit && hit.collider.CompareTag("Robot"))
                {
                    m_hackedObject = hit.collider.gameObject.GetComponent<Robot>();
                    LogicInit(m_hackedObject.GetLevel(), m_hackedObject.GetInterupt(), m_hackedObject.GetAntiVirusDifficulty());
                }

                if (objectHit && hit.collider.CompareTag("HighSecurityControlPanel"))
                {
                    HighSecurityControlPanel panel = hit.collider.gameObject.GetComponent<HighSecurityControlPanel>();
                    if (!panel.IsPhysical())
                    {
                        m_hackedObject = panel;
                        LogicInit(m_hackedObject.GetLevel(), m_hackedObject.GetInterupt(), m_hackedObject.GetAntiVirusDifficulty());

                    }
                }

                if (objectHit && hit.collider.CompareTag("Computer"))
                {
                    Computer computer = hit.collider.gameObject.GetComponent<Computer>();
                    computer.SetPlayerController(this);
                    m_hackedObject = computer;
                    LogicInit(m_hackedObject.GetLevel(), m_hackedObject.GetInterupt(), m_hackedObject.GetAntiVirusDifficulty());
                }
            }
            // For robots.
            else if (!m_inCamera)
            {
                bool objectHit = Physics.Raycast(cameraRay, out RaycastHit hit, 1.0f);

                if (objectHit && hit.collider.CompareTag("DoorLock"))
                {
                    DoorLock doorLock = hit.collider.gameObject.GetComponent<DoorLock>();
                    if (doorLock.IsPhysical())
                    {
                        m_hackedObject = hit.collider.gameObject.GetComponent<DoorLock>();
                        LogicInit(m_hackedObject.GetLevel(), m_hackedObject.GetInterupt(), m_hackedObject.GetAntiVirusDifficulty());
                    }
                }

                if (objectHit && hit.collider.CompareTag("LowSecurityControlPanel"))
                {
                    LowSecurityControlPanel panel = hit.collider.gameObject.GetComponent<LowSecurityControlPanel>();
                    if (panel.IsPhysical())
                    {
                        m_hackedObject = hit.collider.gameObject.GetComponent<LowSecurityControlPanel>();
                        LogicInit(m_hackedObject.GetLevel(), m_hackedObject.GetInterupt(), m_hackedObject.GetAntiVirusDifficulty());
                    }
                }

                if (objectHit && hit.collider.CompareTag("HighSecurityControlPanel"))
                {
                    HighSecurityControlPanel panel = hit.collider.gameObject.GetComponent<HighSecurityControlPanel>();
                    if (panel.IsPhysical())
                    {
                        m_hackedObject = panel;
                        LogicInit(m_hackedObject.GetLevel(), m_hackedObject.GetInterupt(), m_hackedObject.GetAntiVirusDifficulty());
                    }
                }
            }
        }
    }

    // Start the Logic Generator with the parameters provided.
    public void LogicInit(int level, int interupt, int difficulty)
    {
        m_cameraCanvas.SetActive(false);
        m_logicCanvas.SetActive(true);
        m_logicGenerator.StartLogic(level, interupt, difficulty);
        Cursor.lockState = CursorLockMode.Confined;
        if (m_robot != null)
            m_robot.SetCinemachineProfile(m_hackingProfile);
        else
            m_securityCamera.SetCinemachineProfile(m_hackingProfile);
    }

    // Handle the relation between the logic screen and the camera screen once logic has been completed.
    public void LogicComplete()
    {
        if (m_robot != null)
            m_robot.SetCinemachineProfile(m_robotProfile);
        else
            m_securityCamera.SetCinemachineProfile(m_cameraProfile);
        m_cameraCanvas.SetActive(true);
        m_logicCanvas.SetActive(false);
        if (m_hackedObject.IsLevelEnd() && m_hackedObject.GetSecurityState() == 1)
        {
            NextLevel();
            return;
        }
        else if (m_hackedObject.GetType().Name == "Robot")
        {
            m_robot = (Robot)m_hackedObject;
            m_robotController = m_robot.GetCharacterController();
            m_robotHead = m_robot.GetRobotHead();
            m_inCamera = false;
            m_cursor.transform.localPosition = Vector3.zero;
            m_robot.SetPlayerController(this);
            m_effectSource.clip = m_robotEffect;
        }
        else
        {
            m_effectSource.clip = m_cameraEffect;
        }
        m_hackedObject.UnlockOutput();
        bool hasComputer = false;
        if (m_hackedObject.GetType().Name == "Computer" || m_hackedObject.GetType().Name == "ChargingStation")
        {
            string textFileName = null;
            hasComputer = true;
            if (m_hackedObject.GetType().Name == "ChargingStation")
            {
                ChargingStation chargingStation = (ChargingStation)m_hackedObject;
                if (chargingStation.HasComputer())
                {
                    textFileName = chargingStation.GetTextFileName();
                }
                else
                    hasComputer = false;
            }
            else
            {
                Computer computer = (Computer)m_hackedObject;
                textFileName = computer.GetTextFileName();
            }
            m_desktopManager.DisplayDesktop(textFileName);
        }
        if (!hasComputer)
        {
            Cursor.lockState = CursorLockMode.Locked;
            m_hackedObject = null;
        }
    }

    // Return back to the cameras and unlink from the robot.
    public void ExitRobot()
    {
        m_inCamera = true;
        m_robot = null;
        m_robotController = null;
        m_robotHead = null;
        m_effectSource.clip = m_cameraEffect;
    }

    // Update the time and date text on the camera every second.
    IEnumerator UpdateTime()
    {
        while (true)
        {
            m_timeText.text = "" + System.DateTime.Now.ToString("G");
            m_liveText.enabled = !m_liveText.enabled;
            yield return new WaitForSecondsRealtime(1.0f);
        }
    }

    // Make the VCR volume sharply increase in noise before gradually decreasing.
    IEnumerator CreateNoise()
    {
        m_effectSource.PlayOneShot(m_whiteNoiseEffect);
        while (m_VCRVolume.Noisy.value < 1.5)
        {
            m_VCRVolume.Noisy.value += 0.2f;
            yield return new WaitForSecondsRealtime(0.002f);
        }
        while (m_VCRVolume.Noisy.value > 0.1)
        {
            m_VCRVolume.Noisy.value -= 0.02f;
            yield return new WaitForSecondsRealtime(0.002f);
        }
    }

    public void PlayGame()
    {
        m_inMenu = false;
        m_startCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void TogglePauseMenuButton(InputAction.CallbackContext value)
    {
        if (value.started && !m_startCanvas.activeSelf && !m_deathCanvas.activeSelf)
        {
            ButtonPress();
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        if (m_inMenu)
        {
            if (!m_inCamera)
            {
                if (m_hackedObject != null)
                    m_robot.SetCinemachineProfile(m_hackingProfile);
                else
                    m_robot.SetCinemachineProfile(m_robotProfile);
            }
            else
            {
                if (m_hackedObject != null)
                    m_securityCamera.SetCinemachineProfile(m_hackingProfile);
                else
                    m_securityCamera.SetCinemachineProfile(m_cameraProfile);
            }
            m_inMenu = false;
            m_audioManager.PlayBackgroundGameMusic();
            m_pauseCanvas.SetActive(false);
            m_tutorialMenu.SetActive(false);
            m_pauseMenu.SetActive(true);
            if (m_hackedObject == null)
                Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
        else
        {
            if (m_inCamera)
                m_securityCamera.SetCinemachineProfile(m_menuProfile);
            else
                m_robot.SetCinemachineProfile(m_menuProfile);
            m_inMenu = true;
            m_audioManager.PlayMainMenuScreenMusic();
            m_pauseCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;
        }
    }

    public void KillPlayer(bool inLogic)
    {
        m_inMenu = true;
        m_audioManager.PlayDeathScreenMusic();
        m_audioManager.PlayJumpscareClip();
        if (inLogic)
            m_logicCanvas.SetActive(false);
        m_deathCanvas.SetActive(true);
        m_deathAnimator.StartAnimator();
        Cursor.lockState = CursorLockMode.Confined;
        if (m_inCamera)
            m_securityCamera.SetCinemachineProfile(m_menuProfile);
        else
            m_robot.SetCinemachineProfile(m_menuProfile);
    }

    public void RestartLevelFromUI()
    {
        StartCoroutine(RestartLevel());
    }

    public IEnumerator RestartLevel()
    {
        yield return LoadScene(m_levelIndex);
        if (m_inCamera)
            m_securityCamera.SetCinemachineProfile(m_cameraProfile);
        else
            m_robot.SetCinemachineProfile(m_robotProfile);
        Start();
        ExitRobot();
        m_hackedObject = null;
        m_inMenu = false;
        m_audioManager.PlayBackgroundGameMusic();
        m_deathCanvas.SetActive(false);
        m_deathAnimator.StopAnimator();
        m_cameraCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void NextLevel()
    {
        m_levelIndex++;
        StartCoroutine(RestartLevel());
    }

    public IEnumerator LoadScene(int index)
    {
        AsyncOperation scene = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        yield return new WaitUntil(() => scene.isDone == true);
        yield return null;
    }

    public bool InMenu()
    {
        return m_inMenu;
    }

    public void SetHackedObject(HackableObject hackedObject)
    {
        if (hackedObject == null)
            m_securityCamera.SetCinemachineProfile(m_cameraProfile);
        m_hackedObject = hackedObject;
    }

    public void SetSecurityCamera(SecurityCamera securityCamera)
    {
        StopCoroutine(CreateNoise());
        StartCoroutine(CreateNoise());
        m_nameText.text = securityCamera.name;
        m_cameraStartXRotation = securityCamera.GetStartXRotation();
        m_cameraStartYRotation = securityCamera.GetStartYRotation();
        m_cameraYRotation = m_cameraStartYRotation;
        m_cameraXRotation = m_cameraStartXRotation;
        if (m_securityCamera != null)
            m_securityCamera.ToggleActivation(false);
        m_securityCamera = securityCamera;
        m_securityCamera.ToggleActivation(true);
    }

    public void ToggleTutorialPause(bool value)
    {
        if (value)
        {
            m_pauseMenu.SetActive(false);
            m_tutorialMenu.SetActive(true);
        }
        else
        {
            m_tutorialMenu.SetActive(false);
            m_pauseMenu.SetActive(true);
        }
    }

    public void ToggleTutorialGame(InputAction.CallbackContext value)
    {
        if (value.started && !m_startCanvas.activeSelf && !m_deathCanvas.activeSelf)
        {
            ButtonPress();
            if (m_pauseCanvas.activeSelf)
            {
                if (m_tutorialMenu.activeSelf)
                {
                    m_tutorialMenu.SetActive(false);
                    m_pauseMenu.SetActive(true);
                    TogglePauseMenu();
                }
                else
                {
                    m_pauseMenu.SetActive(false);
                    m_tutorialMenu.SetActive(true);
                }
            }
            else
            {
                TogglePauseMenu();
                m_pauseMenu.SetActive(false);
                m_tutorialMenu.SetActive(true);
            }
        }
    }

    public void ButtonPress()
    {
        m_effectSource.PlayOneShot(m_buttonPressEffect);
    }
}
