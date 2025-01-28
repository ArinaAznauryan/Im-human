using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;


public class InputManager : MonoBehaviour
{

    public float mouseDragPhysicsSpeed = 10f, mouseDragSpeed = 0.1f;

    public Vector3 velocity;
    public CinemachineVirtualCamera cineCamera;

    PlayerInput playerInput;
    public InputAction mouseClick;

    public LayerMask mask;

    public MobileInputSys touchControls;
    public GameObject leftButtom, worldFollower;
    Vector3 startPos, endPos, tapStartPos, tapEndPos;
    public Canvas canvas;
    public Vector2 joyMovePos, joyWorldMovePos;
    public Vector2 prevPos;
    public bool tapNum;
    private Coroutine zoomCoroutine;
    private Transform cameraTransform;
    public float speed = 0.01f; 
	private float prevMagnitude = 0;
	private int touchCount = 0;

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();


    Vector2 startDrag, endDrag;

    public bool clickUp = false, clickDown = false, tapped = false, fastTapped = false, dragged = false, placeJoystick = false, joystickActive = false;

    bool startDragDetect = false, tap = false;

    protected static bool isRegistered = false;
    private bool didIRegister = false;

    public bool InputTapped() {
        return fastTapped;
    }

    private void Awake() {
        velocity = Vector3.zero;

        touchControls = new MobileInputSys();

    }
    private void OnEnable() {
        mouseClick.performed += MousePressed;
        mouseClick.Enable();
        Debug.Log("ENABLED");

        if (!isRegistered)
        {
            touchControls.Enable();
            isRegistered = true;
            didIRegister = true;
            
        }   
    }
    private void OnDisable() {
        Debug.Log("DISABLED");

        mouseClick.performed += MousePressed;
        mouseClick.Disable();

        if (didIRegister)
        {
            isRegistered = false;
            didIRegister = false;
            touchControls.Disable();
        }

    }
 

    void MousePressed(InputAction.CallbackContext context) { 
        Vector3 pos = Camera.main.WorldToScreenPoint(worldFollower.transform.position);
        Ray ray = Camera.main.ScreenPointToRay(pos);
        var hit2D = Physics2D.GetRayIntersection(ray);
        
        if (hit2D) { 
            if (hit2D.collider != null) {  
            } 
        }

       
    }

    private void Start(){
        playerInput = GetComponent<PlayerInput>();
        touchControls.playerMovement.TouchPress.started += ctx => tapStart(ctx);
        touchControls.playerMovement.TouchPress.canceled += ctx => tapEnd(ctx);

        touchControls.playerMovement.FirstTouchContact.performed += _ => touchCount++;
		touchControls.playerMovement.SecondTouchContact.performed += _ => touchCount++;

		touchControls.playerMovement.FirstTouchContact.canceled += _ => 
		{
			touchCount--;
			prevMagnitude = 0;
		};
		touchControls.playerMovement.SecondTouchContact.canceled += _ => 
		{
			touchCount--;
			prevMagnitude = 0;
		};

		touchControls.playerMovement.firstTouchPosition.performed += _ => 
		{
			if(touchCount < 2)
				return;
			var magnitude = (touchControls.playerMovement.firstTouchPosition.ReadValue<Vector2>() - touchControls.playerMovement.secondTouchPosition.ReadValue<Vector2>()).magnitude;
			if(prevMagnitude == 0)
				prevMagnitude = magnitude;
			var difference = magnitude - prevMagnitude;
			prevMagnitude = magnitude;
			CameraZoom(-difference * speed);
		};
    }

    private void CameraZoom(float increment) => cineCamera.m_Lens.OrthographicSize = Mathf.Clamp(cineCamera.m_Lens.OrthographicSize + increment, 20, 60);


    private void tapStart(InputAction.CallbackContext context) {
        tap = true;
        Debug.Log("CLICKED");
        tapStartPos = touchControls.playerMovement.TouchPosition.ReadValue<Vector2>();
    }
    private void tapEnd(InputAction.CallbackContext context) {
        tap = false;
        Debug.Log("UNCLICKED");
        fastTapped = true;
        tapEndPos = touchControls.playerMovement.TouchPosition.ReadValue<Vector2>();
    }

    private void StartTouch(InputAction.CallbackContext context) {
        clickUp = false;
        clickDown = true;

        dragged = true;
    }
    private void EndTouch(InputAction.CallbackContext context) {
        clickUp = true;
        clickDown = false;

        dragged = false;
    
        if (!float.IsInfinity(touchControls.playerMovement.TouchPosition.ReadValue<Vector2>().x) || !float.IsInfinity(touchControls.playerMovement.TouchPosition.ReadValue<Vector2>().y))
        {
            endPos = Camera.main.ScreenToWorldPoint(touchControls.playerMovement.TouchPosition.ReadValue<Vector2>());
        }
    }
    void Update() {

        Vector3 pos = Camera.main.WorldToScreenPoint(worldFollower.transform.position);
        Ray ray = Camera.main.ScreenPointToRay(pos);
        var hit2D = Physics2D.GetRayIntersection(ray);

        if (tap) {
            tapped = false;
        }

        endDrag = playerInput.actions["TouchPosition"].ReadValue<Vector2>();

        touchControls.playerMovement.TouchMove.started += ctx => StartTouch(ctx);
        touchControls.playerMovement.TouchMove.canceled += ctx => EndTouch(ctx);

        if (!float.IsInfinity(playerInput.actions["TouchPosition"].ReadValue<Vector2>().x) || !float.IsInfinity(playerInput.actions["TouchPosition"].ReadValue<Vector2>().y))
        {
            joyMovePos = playerInput.actions["TouchPosition"].ReadValue<Vector2>();
        }

        joyWorldMovePos = new Vector2(Camera.main.ScreenToWorldPoint(joyMovePos).x, Camera.main.ScreenToWorldPoint(joyMovePos).y);
        
        if (tapStartPos == tapEndPos && !tap) {
            tap = true;
            tapped = true;
        }
    
        else if (tapStartPos != tapEndPos && tap) {
            tapped = false;
        }
        
        worldFollower.transform.position = joyWorldMovePos;
       
    }

    void LateUpdate(){
        if (fastTapped) fastTapped = false;
    }
   
}