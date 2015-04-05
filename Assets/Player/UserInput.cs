using UnityEngine;
using System.Collections;
using ResourceManager;

public class UserInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		MoveCamera();
		RotateCamera();
	}

	private void MoveCamera(){
		Vector2 cursorLocation = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		
		Vector3 movement = new Vector3(0,0,0);

		//horizontal camera movement
		if(cursorLocation.x >= 0 && cursorLocation.x < RM.ScrollWidth || Input.GetKey("a")) {
			movement.x -= RM.MoveSpeed;
		} else if(cursorLocation.x <= Screen.width && cursorLocation.x > Screen.width - RM.ScrollWidth || Input.GetKey("d")) {
			movement.x += RM.MoveSpeed;
		}
		
		//vertical camera movement
		if(cursorLocation.y >= 0 && cursorLocation.y < RM.ScrollWidth || Input.GetKey("s")) {
			movement.z -= RM.MoveSpeed;
		} else if(cursorLocation.y <= Screen.height && cursorLocation.y > Screen.height - RM.ScrollWidth || Input.GetKey("w")) {
			movement.z += RM.MoveSpeed;
		}

		//convert from local space to world space
		movement = Camera.main.transform.TransformDirection(movement);
		//we don't want camera changing height (y)
		movement.y = 0;
		//but we do want, if user scrolls
		movement.y -= RM.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel");


		//calculate desired camera position based on received input
		Vector3 origin = Camera.main.transform.position;
		Vector3 destination = origin;
		destination += movement;

		//limit away from ground movement to be between a minimum and maximum distance
		if(destination.y > RM.MaxCameraHeight) {
			destination.y = RM.MaxCameraHeight;
		} else if(destination.y < RM.MinCameraHeight) {
			destination.y = RM.MinCameraHeight;
		}

		//if a change in position is detected perform the necessary update
		if(destination != origin) {
			Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * RM.ScrollSpeed);
		}
	}

	private void RotateCamera(){

	}
}
