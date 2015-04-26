using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AntMovement))]

public class Ant : MonoBehaviour  {
	private float health;
	private float hungerLevel;
	private float age;
	private List<Vector3> destinationList;
	private Vector3 homeCoordinates;

	public Ant(){
		destinationList = new List<Vector3> ();
	}

	public virtual void Start(){
		
	}
	
	public virtual void Update () {
		
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

	void OnCollisionEnter(Collision col){
		
	}

	void OnCollisionStay(Collision col){

	}
	
	void OnCollisionExit(Collision col){
		
	}
}
