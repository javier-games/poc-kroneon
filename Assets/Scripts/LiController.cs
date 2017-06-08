using System;
using UnityEngine;

namespace Kroneon.Li
{
	[RequireComponent(typeof (LiMovement))]

	public class LiController : MonoBehaviour{


		//References
		private LiMovement	mov;				// A reference to LiMovement component on the object to sent the move vector.
		private Transform 	cam;            	// A reference to the main camera in the scenes transform.

		//Variables
		private Vector3 camForward;          	// The current forward direction of the camera.
		private Vector3 dir;					// Vector of the movement of Li, regarding to the main camera and user input.
		private bool 	jump;					// User input for jump
		private bool 	hold;					// User input for hold.

		//Variables of time travel
		float time_i = 0;						// Initial time of the play process
		LiTravel paths;							// This should be a list of LiTravel
		bool showList = false;					// Debuging Flag
		[SerializeField] LiMovement path_past;	// Slave LiCharacter


		private void Start(){

			// Getting the references.

			// Get the transform of the main camera
			if (Camera.main != null){
				cam = Camera.main.transform;
			}
			else{
				Debug.LogWarning("Warning: no main camera found.", gameObject);
			}

			// Get the Li character
			mov = GetComponent<LiMovement>();

			time_i = Time.time;
			paths = new LiTravel ();
		}


		// Fixed update is called in sync with physics
		private void FixedUpdate(){
			
			// Read Inputs
			float h   = Mathf.Round( Input.GetAxis("Horizontal") * 10 )/10;
			float v   = Mathf.Round( Input.GetAxis("Vertical"  ) * 10 )/10;
			hold = Input.GetKey(KeyCode.RightShift);
			jump    = Input.GetButtonDown("Jump");

			float time_c = Time.time-time_i;

			// Calculate move direction to pass to character
			if (cam != null){
				// Calculate camera relative direction to move
				camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
				dir = v*camForward + h*cam.right;
			}
			else{
				// Using world-relative directions in the case of no main camera
				dir = v*Vector3.forward + h*Vector3.right;
			}
				
			// Pass all parameters to the character control script
			mov.Move(dir, hold, jump);


			// Debuging /*
			if (!showList) {

				// Storing an action
				paths.AddAction(dir,mov.gameObject.GetComponent<Transform>().position,time_c, hold, jump);
		
				if (Input.GetKey (KeyCode.Tab)) {
					paths.ShowList ();
					showList = true;
					paths.SetFinalTime (time_c);
					path_past.gameObject.SetActive (true);
				}
			} else {

				//Update to the aprox time
				paths.TimeUpdate (time_c);
				//Update the position
				paths.PositionUpdate (path_past);
				//Do the movement.
				path_past.Move (paths.GetPath ().GetDirection (), paths.GetPath ().GetHold (), paths.GetPath ().GetJump ());
			
			}
			// */

		}
	}
}
