using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    public LogicGenerator logicGenerator;
    public Camera mainCamera;

    public GameObject cameraCanvas;
    public GameObject cursor;
    public TextMeshProUGUI currentTime;
    public TextMeshProUGUI currentName;
    public Slider currentZoom;

    public GameObject logicCanvas;

    public GameObject startCanvas;
    public GameObject deathCanvas;
    public GameObject pauseCanvas;

    public VolumeProfile cameraProfile;
    public VolumeProfile hackingProfile;
    public VolumeProfile menuProfile;
    public VolumeProfile robotProfile;
    private VCRVolume currentVCR;
    private CRTVolume currentTestVol;

    private bool inCamera = true;
    public SecurityCamera currentCamera;
    private HackableObject hackedObject;
    private Robot currentRobot;
    private CharacterController robotCharCon;
    private Transform robotHead;

    private static float moveSpeed = 25;
    public float startXRotation;
    public float startYRotation;
    public float currentRotation;
    private Vector2 currentMove;

    private bool inMenu = true;

    // Don't destroy the player controller or main camera.
    public void Awake()
    {
        DontDestroyOnLoad(mainCamera);
        DontDestroyOnLoad(gameObject);
    }

    // Setup movement for first security camera.
    public void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        startXRotation = currentCamera.GetStartXRotation();
        startYRotation = currentCamera.GetStartYRotation();
        currentRotation = startYRotation;
        StartCoroutine(UpdateTime());
        cameraProfile.TryGet<VCRVolume>(out currentVCR);
    }

    // Handle the security camera and robot movement.
    public void Update()
    {
        if (hackedObject == null && inMenu == false)
        {
            // For cameras.
            if (inCamera)
            {
                // Use the y axis on currentMove to control camera zoom (FOV).
                float yMove = currentMove[1] * Time.deltaTime * moveSpeed;
                if (yMove > 0.01 || yMove < -0.01)
                {
                    float fov = currentCamera.GetCinemachineCamera().m_Lens.FieldOfView;
                    if (fov - yMove < 60 && fov - yMove > 15)
                    {
                        currentCamera.GetCinemachineCamera().m_Lens.FieldOfView -= yMove;
                        currentZoom.value -= yMove;
                    }
                }

                // Use the x and y axis on currentLook to control cursor.
                Vector3 newLook = new Vector3(Mouse.current.delta.ReadValue().x, Mouse.current.delta.ReadValue().y);
                if (cursor.transform.localPosition.x + newLook.x > Screen.width / 2 || cursor.transform.localPosition.x + newLook.x < -Screen.width / 2)
                    newLook.x = 0;
                if (cursor.transform.localPosition.y + newLook.y > Screen.height / 2 || cursor.transform.localPosition.y + newLook.y < -Screen.height / 2)
                    newLook.y = 0;
                cursor.transform.localPosition += newLook;

                // Use the x axis on currentMove to control camera y rotation.
                float xMove = currentMove[0] * Time.deltaTime * moveSpeed;
                if (xMove > 0.01 || xMove < -0.01)
                {
                    if (currentRotation + xMove < 30 + startYRotation && currentRotation + xMove > -30 + startYRotation)
                        currentRotation += xMove;
                }

                currentCamera.gameObject.transform.localRotation = Quaternion.Euler((-cursor.transform.localPosition.y / Screen.height * 9) + startXRotation, (cursor.transform.localPosition.x / Screen.width * 16) + currentRotation, 0f);
            }
            // For robots.
            else
            {
                // Use the x and y axis on currentLook to control cursor.
                Vector3 newRobotLook = new Vector3(0, Mouse.current.delta.ReadValue().x / 10);
                currentRobot.transform.Rotate(newRobotLook);

                Vector3 newHeadLook = new Vector3(-Mouse.current.delta.ReadValue().y / 10, 0);

                if (robotHead.localEulerAngles.x + newHeadLook.x > 50 && robotHead.localEulerAngles.x + newHeadLook.x < 310)
                    newHeadLook = new Vector3(0, 0);
                robotHead.transform.Rotate(newHeadLook);

                Vector3 move = new Vector3(currentMove[1], 0, -currentMove[0]);
                robotCharCon.Move(currentRobot.transform.TransformDirection(move * Time.deltaTime * 2));
            }
        }
    }

    // This method uses the wasd keys.
    public void Move(InputAction.CallbackContext value)
    {
        //Store the most recent Vector2 value from the input.
        currentMove = value.ReadValue<Vector2>();
    }

    // When the interact button is clicked, find out if there was something that was clicked on.
    public void Interact(InputAction.CallbackContext value)
    {
        if (value.started && hackedObject == null && inMenu == false)
        {
            Ray cameraRay = mainCamera.ScreenPointToRay(new Vector3(cursor.transform.localPosition.x + (Screen.width / 2), cursor.transform.localPosition.y + (Screen.height / 2), 0));

            // For cameras.
            if (inCamera)
            {
                bool objectHit = Physics.Raycast(cameraRay, out RaycastHit hit);

                if (objectHit && hit.collider.CompareTag("SecurityCamera"))
                {
                    StopCoroutine(CreateNoise());
                    StartCoroutine(CreateNoise());
                    SecurityCamera previousCamera = currentCamera;
                    currentCamera = hit.collider.gameObject.GetComponent<SecurityCamera>();
                    startXRotation = currentCamera.GetStartXRotation();
                    startYRotation = currentCamera.GetStartYRotation();
                    currentRotation = startYRotation;

                    previousCamera.ToggleActivation(false);
                    currentCamera.ToggleActivation(true);

                    currentName.text = hit.collider.name;
                    currentZoom.value = currentCamera.GetCinemachineCamera().m_Lens.FieldOfView;
                }

                if (objectHit && hit.collider.CompareTag("DoorLock"))
                {
                    hackedObject = hit.collider.gameObject.GetComponent<DoorLock>();
                    LogicInit(hackedObject.GetLevel(), hackedObject.GetInterupt(), hackedObject.GetAntiVirusDifficulty());
                }

                if (objectHit && hit.collider.CompareTag("LowSecurityControlPanel"))
                {
                    hackedObject = hit.collider.gameObject.GetComponent<LowSecurityControlPanel>();
                    LogicInit(hackedObject.GetLevel(), hackedObject.GetInterupt(), hackedObject.GetAntiVirusDifficulty());
                }

                if (objectHit && hit.collider.CompareTag("Robot"))
                {
                    hackedObject = hit.collider.gameObject.GetComponent<Robot>();
                    LogicInit(hackedObject.GetLevel(), hackedObject.GetInterupt(), hackedObject.GetAntiVirusDifficulty());
                }

                if (objectHit && hit.collider.CompareTag("HighSecurityControlPanel"))
                {
                    HighSecurityControlPanel panel = hit.collider.gameObject.GetComponent<HighSecurityControlPanel>();
                    if (panel.GetSecurityState() == 1)
                    {
                        hackedObject = panel;
                        LogicInit(hackedObject.GetLevel(), hackedObject.GetInterupt(), hackedObject.GetAntiVirusDifficulty());

                    }
                }
            }
            // For robots.
            else if (!inCamera)
            {
                bool objectHit = Physics.Raycast(cameraRay, out RaycastHit hit, 1.0f);

                if (objectHit && hit.collider.CompareTag("HighSecurityControlPanel"))
                {
                    HighSecurityControlPanel panel = hit.collider.gameObject.GetComponent<HighSecurityControlPanel>();
                    if (panel.GetSecurityState() == 2)
                    {
                        hackedObject = panel;
                        LogicInit(hackedObject.GetLevel(), hackedObject.GetInterupt(), hackedObject.GetAntiVirusDifficulty());
                    }
                }
            }
        }
    }

    // Start the Logic Generator with the parameters provided.
    public void LogicInit(int level, int interupt, int difficulty)
    {
        cameraCanvas.SetActive(false);
        logicCanvas.SetActive(true);
        logicGenerator.StartLogic(level, interupt, difficulty);
        Cursor.lockState = CursorLockMode.Confined;
        if (currentRobot != null)
            currentRobot.SetCinemachineProfile(hackingProfile);
        else
            currentCamera.SetCinemachineProfile(hackingProfile);
    }

    // Handle the relation between the logic screen and the camera screen once logic has been completed.
    public void LogicComplete()
    {
        if (currentRobot != null)
            currentRobot.SetCinemachineProfile(robotProfile);
        else
            currentCamera.SetCinemachineProfile(cameraProfile);
        cameraCanvas.SetActive(true);
        logicCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        if (hackedObject.GetType().Name == "Robot")
        {
            currentRobot = (Robot)hackedObject;
            robotCharCon = currentRobot.GetCharacterController();
            robotHead = currentRobot.GetRobotHead();
            inCamera = false;
            cursor.transform.localPosition = Vector3.zero;
            currentRobot.SetPlayerController(this);
        }
        hackedObject.UnlockOutput();
        hackedObject = null;
    }

    // Return back to the cameras and unlink from the robot.
    public void ExitRobot()
    {
        inCamera = true;
        currentRobot = null;
        robotCharCon = null;
        robotHead = null;
    }

    // Update the time and date text on the camera every second.
    IEnumerator UpdateTime()
    {
        while (true)
        {
            currentTime.text = "" + System.DateTime.Now.ToString("G");
            yield return new WaitForSecondsRealtime(1.0f);
        }
    }

    // Make the VCR volume sharply increase in noise before gradually decreasing.
    IEnumerator CreateNoise()
    {
        while (currentVCR.Noisy.value < 1.5)
        {
            currentVCR.Noisy.value += 0.2f;
            yield return new WaitForSecondsRealtime(0.002f);
        }
        while (currentVCR.Noisy.value > 0.1)
        {
            currentVCR.Noisy.value -= 0.02f;
            yield return new WaitForSecondsRealtime(0.002f);
        }
    }

    public void PlayGame()
    {
        inMenu = false;
        startCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void TogglePauseMenu(InputAction.CallbackContext value)
    {
        menuProfile.TryGet<CRTVolume>(out currentTestVol);
        if (value.started)
        {
            if (inMenu)
            {
                if (!inCamera)
                {
                    if (hackedObject != null)
                        currentRobot.SetCinemachineProfile(hackingProfile);
                    else
                        currentRobot.SetCinemachineProfile(robotProfile);
                }
                else
                {
                    if (hackedObject != null)
                        currentCamera.SetCinemachineProfile(hackingProfile);
                    else
                        currentCamera.SetCinemachineProfile(cameraProfile);
                }
                inMenu = false;
                pauseCanvas.SetActive(false);
                if (hackedObject == null)
                    Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
            }
            else
            {
                if (inCamera)
                    currentCamera.SetCinemachineProfile(menuProfile);
                else
                    currentRobot.SetCinemachineProfile(menuProfile);
                inMenu = true;
                pauseCanvas.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
                Time.timeScale = 0;
            }
        }
    }

    public bool InMenu()
    {
        return inMenu;
    }
}
