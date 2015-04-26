using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Ant))]

public class AntMovement : MonoBehaviour {
	private Ant ant;

	// Use this for initialization
	void Start () {
		ant = GetComponent<Ant> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		SetUpDir ();
	}

	//move towards a target at a set speed.
	private void MoveTowardsTarget() {
		//the speed, in units per second, we want to move towards the target
		float speed = 1;
		//move towards the center of the world (or where ever you like)
		Vector3 targetPosition = new Vector3(0,0,0);
		
		Vector3 currentPosition = this.transform.position;
		//first, check to see if we're close enough to the target
		if(Vector3.Distance(currentPosition, targetPosition) > .1f) { 
			Vector3 directionOfTravel = targetPosition - currentPosition;
			//now normalize the direction, since we only want the direction information
			directionOfTravel.Normalize();
			//scale the movement on each axis by the directionOfTravel vector components
			
			this.transform.Translate(
				(directionOfTravel.x * speed * Time.deltaTime),
				(directionOfTravel.y * speed * Time.deltaTime),
				(directionOfTravel.z * speed * Time.deltaTime),
				Space.World);
		}
	}

	
	public virtual void SetUpDir () {
		RaycastHit raycastHit;
		
		Physics.Raycast(transform.position, Vector3.down, out raycastHit);
		transform.position = new Vector3 (raycastHit.point.x, raycastHit.point.y + 0.6f * transform.localScale.y, raycastHit.point.z);
		
		Vector3 backLeft;
		Vector3 backRight;
		Vector3 frontLeft;
		Vector3 frontRight;
		RaycastHit lr;
		RaycastHit rr;
		RaycastHit lf;
		RaycastHit rf;
		
		Vector3 upDir;
		
		backLeft = transform.position - 0.7f * transform.forward*transform.localScale.x - 0.7f * transform.right*transform.localScale.z;
		backRight = transform.position - 0.7f * transform.forward*transform.localScale.x + 0.7f * transform.right*transform.localScale.z;
		frontLeft = transform.position + 0.7f * transform.forward*transform.localScale.x - 0.7f * transform.right*transform.localScale.z;
		frontRight = transform.position + 0.7f * transform.forward*transform.localScale.x + 0.7f * transform.right*transform.localScale.z;
		//		Debug.DrawLine (backLeft, backRight, Color.red);
		//		Debug.DrawLine (backLeft, frontLeft, Color.red);
		//		Debug.DrawLine (backRight, frontRight, Color.red);
		//		Debug.DrawLine (frontLeft, frontRight, Color.red);
		
		Physics.Raycast(backLeft + Vector3.up, Vector3.down, out lr);
		Physics.Raycast(backRight + Vector3.up, Vector3.down, out rr);
		Physics.Raycast(frontLeft + Vector3.up, Vector3.down, out lf);
		Physics.Raycast(frontRight + Vector3.up, Vector3.down, out rf);
		
		upDir = (Vector3.Cross(rr.point - Vector3.up, lr.point - Vector3.up) +
		         Vector3.Cross(lr.point - Vector3.up, lf.point - Vector3.up) +
		         Vector3.Cross(lf.point - Vector3.up, rf.point - Vector3.up) +
		         Vector3.Cross(rf.point - Vector3.up, rr.point - Vector3.up)
		         ).normalized;
		//		Debug.DrawRay(rr.point, Vector3.up);
		//		Debug.DrawRay(lr.point, Vector3.up);
		//		Debug.DrawRay(lf.point, Vector3.up);
		//		Debug.DrawRay(rf.point, Vector3.up);
		
		transform.up = upDir;
		
	}

}
