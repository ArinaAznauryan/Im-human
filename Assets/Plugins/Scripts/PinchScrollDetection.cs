// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;
// public class PinchScrollDetection : MonoBehaviour 
// {
// 	public float speed = 0.01f;
// 	private float prevMagnitude = 0;
// 	private int touchCount = 0;
// 	private void Start () 
// 	{
// 		// mouse scroll
// 		// var scrollAction = new InputAction(binding: "<Mouse>/scroll");
// 		// scrollAction.Enable();
// 		// scrollAction.performed += ctx => CameraZoom(ctx.ReadValue<Vector2>().y * speed);

// 		// pinch gesture
// 		// var touch0contact = new InputAction
// 		// (
// 		// 	type: InputActionType.Button,
// 		// 	binding: "<Touchscreen>/touch0/press"
// 		// );
// 		// touch0contact.Enable();
// 		// var touch1contact = new InputAction
// 		// (
// 		// 	type: InputActionType.Button,
// 		// 	binding: "<Touchscreen>/touch1/press"
// 		// );
// 		// touch1contact.Enable();

// 		touchControls.playerMovement.FirstTouchContact.performed += _ => touchCount++;
// 		touchControls.playerMovement.SecondTouchContact.performed += _ => touchCount++;
// 		touch0contact.canceled += _ => 
// 		{
// 			touchCount--;
// 			prevMagnitude = 0;
// 		};
// 		touch1contact.canceled += _ => 
// 		{
// 			touchCount--;
// 			prevMagnitude = 0;
// 		};

// 		touchControls.playerMovement.firstTouchPosition.performed += _ => 
// 		{
// 			if(touchCount < 2)
// 				return;
// 			var magnitude = (touchControls.playerMovement.firstTouchPosition.ReadValue<Vector2>() - touchControls.playerMovement.secondTouchPosition.ReadValue<Vector2>()).magnitude;
// 			if(prevMagnitude == 0)
// 				prevMagnitude = magnitude;
// 			var difference = magnitude - prevMagnitude;
// 			prevMagnitude = magnitude;
// 			CameraZoom(-difference * speed);
// 		};
// 	}


	
// }
