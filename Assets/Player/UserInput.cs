using UnityEngine;
using System.Collections;
using ResourceManager;

public class UserInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//Camera.main.transform.position = new Vector3 (RM.TerrainMesh.Width/2,180f,-10);
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
		if(cursorLocation.x >= 0 && cursorLocation.x < RM.Camera.scrollWidth || Input.GetKey("a")) {
			movement.x -= RM.Camera.moveSpeed;
		} else if(cursorLocation.x <= Screen.width && cursorLocation.x > Screen.width - RM.Camera.scrollWidth || Input.GetKey("d")) {
			movement.x += RM.Camera.moveSpeed;
		}
		
		//vertical camera movement
		if(cursorLocation.y >= 0 && cursorLocation.y < RM.Camera.scrollWidth || Input.GetKey("s")) {
			movement.z -= RM.Camera.moveSpeed;
		} else if(cursorLocation.y <= Screen.height && cursorLocation.y > Screen.height - RM.Camera.scrollWidth || Input.GetKey("w")) {
			movement.z += RM.Camera.moveSpeed;
		}

		//convert from local space to world space
		movement = Camera.main.transform.TransformDirection(movement);
		//we don't want camera changing height (y)
		movement.y = 0;
		//but we do want, if user scrolls
		movement.y -= RM.Camera.scrollWidth * Input.GetAxis("Mouse ScrollWheel");


		//calculate desired camera position based on received input
		Vector3 origin = Camera.main.transform.position;
		Vector3 destination = origin;
		destination += movement;

		//limit away from ground movement to be between a minimum and maximum distance
		if(destination.y > RM.Camera.maxCameraHeight) {
			destination.y = RM.Camera.maxCameraHeight;
		} else if(destination.y < RM.Camera.minCameraHeight) {
			destination.y = RM.Camera.minCameraHeight;
		}

		//if a change in position is detected perform the necessary update
		if(destination != origin) {
			Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * RM.Camera.scrollSpeed);
		}
	}

	private void RotateCamera(){

	}
}
