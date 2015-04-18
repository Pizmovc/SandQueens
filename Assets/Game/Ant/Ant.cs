using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]

public class Ant : MonoBehaviour  {
	private float health;
	private float hungerLevel;
	private float age;
	private List<Vector3> destinationList;
	private Vector3 homeCoordinates;
	private float speed = 5;

	public Ant(){
		destinationList = new List<Vector3> ();
	}

	public virtual void SetHomeCoordinates(Vector2 home){
		homeCoordinates = home;
	}

	public virtual Vector2 GetHomeCoordinates(){
		return(homeCoordinates);
	}

	public virtual void AddDestination(Vector3 destination){
		//Debug.Log ("Destination: " + destination);
		destinationList.Add(destination);
	}

	public virtual Vector3 GetDestination(){
		if (destinationList.Count == 0)
			return(homeCoordinates);
		else
			return destinationList [0];
	}

	public virtual void Eat(){
		//reduce amount of food by X (more if queen, or fighter, less if worker, nursery ant)
	}

	public virtual void Start(){
		gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		GetComponent<Rigidbody> ().drag = 5;
		GetComponent<Rigidbody> ().mass = 5;
	}

	public virtual void Update () {
		
	}

	public virtual void FixedUpdate(){
		if(Vector3.Distance(transform.position, GetDestination()) > 1)
			GoToDestination ();
	}

	public virtual void GoToDestination(){
		Vector3 destination = GetDestination();
		destination.y = transform.position.y;
		transform.LookAt (destination);
		GetComponent<Rigidbody>().AddRelativeForce(destination * speed * 0.1f ,ForceMode.Force);
	}
}
