using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{
    public LogicGenerator logicGenerator;
    public GameObject cameraCanvas;
    public GameObject logicCanvas;

    public HackableObject hackedObject;

    public bool inCamera;
    public Robot currentRobot;
    public CharacterController robotCharCon;
    public Transform robotHead;

    public Camera mainCamera;
    public SecurityCamera currentCamera;
    public GameObject cursor;
    public TextMeshProUGUI currentTime;
    public TextMeshProUGUI currentName;
    public Slider currentZoom;
    public float moveSpeed;
    public float startXRotation;
    public float startYRotation;
    public float currentRotation;
    public float lookSpeed;
    private Vector2 currentMove;
    private Vector2 currentControllerLook;
    private Vector2 currentMouseLook;

    public void Awake()
    {
        inCamera = true;
        DontDestroyOnLoad(mainCamera);
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        startXRotation = currentCamera.GetStartXRotation();
        startYRotation = currentCamera.GetStartYRotation();
        currentRotation = startYRotation;
        StartCoroutine(UpdateTime());
    }

    public void Update()
    {
        if (hackedObject == null)
        {
            if (inCamera)
            {
                //Use the y axis on currentMove to control camera zoom (FOV).
                float yMove = currentMove[1] * Time.deltaTime * moveSpeed;
                if (yMove > 0.01 || yMove < -0.01)
                {
                    //Debug.Log(currentCamera.GetCinemachineCamera().m_Lens.FieldOfView - yMove);
                    if (currentCamera.GetCinemachineCamera().m_Lens.FieldOfView - yMove < 60 && currentCamera.GetCinemachineCamera().m_Lens.FieldOfView - yMove > 15)
                    {
                        currentCamera.GetCinemachineCamera().m_Lens.FieldOfView -= yMove;
                        currentZoom.value -= yMove;
                    }
                }

                //Use the x and y axis on currentLook to control cursor.
                float xLook = ((float)currentControllerLook[0] * Time.deltaTime * lookSpeed) + Mouse.current.delta.ReadValue().x;
                float yLook = ((float)currentControllerLook[1] * Time.deltaTime * lookSpeed) + Mouse.current.delta.ReadValue().y;

                Vector3 newLook = new Vector3(xLook, yLook);
                if (cursor.transform.localPosition.x + newLook.x > Screen.width / 2 || cursor.transform.localPosition.x + newLook.x < -Screen.width / 2)
                    newLook = new Vector3(0, newLook.y);
                if (cursor.transform.localPosition.y + newLook.y > Screen.height / 2 || cursor.transform.localPosition.y + newLook.y < -Screen.height / 2)
                    newLook = new Vector3(newLook.x, 0);
                cursor.transform.localPosition += newLook;

                //Use the x axis on currentMove to control camera y rotation.
                float xMove = currentMove[0] * Time.deltaTime * moveSpeed;
                if (xMove > 0.01 || xMove < -0.01)
                {
                    //Debug.Log(currentCamera.GetCamera().transform.localRotation.y * 180);
                    //if (currentCamera.GetCamera().transform.localRotation.y - xMove < 45 + startRotation && currentCamera.GetCamera().transform.localRotation.y - xMove > -45 + startRotation)
                    //    currentRotation += xMove;

                    if (currentRotation + xMove < 30 + startYRotation && currentRotation + xMove > -30 + startYRotation)
                        currentRotation += xMove;
                }

                currentCamera.gameObject.transform.localRotation = Quaternion.Euler((-cursor.transform.localPosition.y / Screen.height * 9) + startXRotation, (cursor.transform.localPosition.x / Screen.width * 16) + currentRotation, 0f);

                //Debug.Log(Mouse.current.delta.ReadValue());
                //Debug.Log(currentMouseLook);

            }
            else
            {
                //Use the x and y axis on currentLook to control cursor.
                float xLook = ((float)currentControllerLook[0] * Time.deltaTime) + Mouse.current.delta.ReadValue().x;
                float yLook = ((float)currentControllerLook[1] * Time.deltaTime) + Mouse.current.delta.ReadValue().y;

                Vector3 newRobotLook = new Vector3(0, xLook / 10);
                currentRobot.transform.Rotate(newRobotLook);

                Vector3 newHeadLook = new Vector3(-yLook / 10, 0);

                if (robotHead.localEulerAngles.x + newHeadLook.x > 50 && robotHead.localEulerAngles.x + newHeadLook.x < 310)
                    newHeadLook = new Vector3(0, 0);
                robotHead.transform.Rotate(newHeadLook);

                Vector3 move = new Vector3(currentMove[1], 0, -currentMove[0]);
                robotCharCon.Move(currentRobot.transform.TransformDirection(move * Time.deltaTime * 2));
            }
        }
    }

    //This method uses the wasd keys and right stick on the gamepad.
    public void Move(InputAction.CallbackContext value)
    {
        //Store the most recent Vector2 value from the input.
        currentMove = value.ReadValue<Vector2>();
    }

    //This method uses the left stick on the gamepad.
    public void Look(InputAction.CallbackContext value)
    {
        //Store the most recent Vector2 value from the input.
        currentControllerLook = value.ReadValue<Vector2>();
    }

    public void Interact(InputAction.CallbackContext value)
    {
        if (value.started && hackedObject == null && inCamera)
        {
            Ray cameraRay = mainCamera.ScreenPointToRay(cursor.transform.position);
            bool objectHit = Physics.Raycast(cameraRay, out RaycastHit hit);

            if (objectHit && hit.collider.tag == "SecurityCamera")
            {
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

            if (objectHit && hit.collider.tag == "DoorLock")
            {
                Debug.Log("Lock Hit");
                hackedObject = hit.collider.gameObject.GetComponent<DoorLock>();
                LogicInit(hackedObject.GetLevel());
            }

            if (objectHit && hit.collider.tag == "LowSecurityControlPanel")
            {
                Debug.Log("LCP Hit");
                hackedObject = hit.collider.gameObject.GetComponent<LowSecurityControlPanel>();
                LogicInit(hackedObject.GetLevel());
            }

            if (objectHit && hit.collider.tag == "Robot")
            {
                Debug.Log("Robot Hit");
                hackedObject = hit.collider.gameObject.GetComponent<Robot>();
                LogicInit(hackedObject.GetLevel());
            }

            if (objectHit && hit.collider.tag == "HighSecurityControlPanel")
            {
                Debug.Log("HCP Hit");
                HighSecurityControlPanel panel = hit.collider.gameObject.GetComponent<HighSecurityControlPanel>();
                if (panel.GetSecurityState() == 1)
                {
                    hackedObject = panel;
                    LogicInit(hackedObject.GetLevel());

                }
            }
        }
        else if (value.started && hackedObject == null && !inCamera)
        {
            Ray cameraRay = mainCamera.ScreenPointToRay(cursor.transform.position);
            bool objectHit = Physics.Raycast(cameraRay, out RaycastHit hit, 1.0f);

            Debug.Log("Robot Click");

            if (objectHit && hit.collider.tag == "HighSecurityControlPanel")
            {
                Debug.Log("HCP Hit");
                HighSecurityControlPanel panel = hit.collider.gameObject.GetComponent<HighSecurityControlPanel>();
                if (panel.GetSecurityState() == 2)
                {
                    hackedObject = panel;
                    LogicInit(hackedObject.GetLevel());

                }
            }
        }
    }

    public void LogicInit(int level)
    {
        cameraCanvas.SetActive(false);
        logicCanvas.SetActive(true);
        logicGenerator.CreateLogic(level);
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void LogicComplete()
    {
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
            currentRobot.LinkController(this);
        }

        hackedObject.UnlockOutput();
        hackedObject = null;
    }

    public void TestLogicInit(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            int level = Random.Range(1, 6);
            level = 4;
            cameraCanvas.SetActive(false);
            logicCanvas.SetActive(true);
            logicGenerator.CreateLogic(level);
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public SecurityCamera GetCurrentSecurityCamera()
    {
        return currentCamera;
    }

    public void ActivateCameraView()
    {
        inCamera = true;
    }

    IEnumerator UpdateTime()
    {
        while (true)
        {
            currentTime.text = "" + System.DateTime.Now.ToString("G");
            yield return new WaitForSeconds(1.0f);
        }
    }
}
