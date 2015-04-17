using UnityEngine;
using System.Collections;
using ResourceManager;

public class QueenAnt : Ant {
	private int successiveAntNumber = 0;
	public float timeBetweenSpawning;
	Vector3 startingPosition = new Vector3 ();


	// Use this for initialization
	public override void Start () {
		startingPosition.x = (float)RM.Terrarium.terrainData.size.x / 10;
		startingPosition.y = (float)RM.Terrarium.height;
		startingPosition.z = (float)RM.Terrarium.terrainData.size.z / 2;
		//Debug.Log(startingPosition);
		transform.position = startingPosition;
		StartCoroutine (spawnAnts(timeBetweenSpawning));
	}
	
	// Update is called once per frame
	public override void Update () {
		transform.position = startingPosition;
	}

	IEnumerator spawnAnts(float delay){
		GameObject ant = GameObject.CreatePrimitive(PrimitiveType.Capsule);
		ant.transform.position = transform.position;
		ant.name = "Ant " + successiveAntNumber.ToString();
		successiveAntNumber ++;
		ant.AddComponent<Ant> ();
		ant.GetComponent<Ant>().AddDestination(new Vector3(Random.Range(0,RM.Terrarium.terrainData.size.x),0, Random.Range(0,RM.Terrarium.terrainData.size.z)));
		yield return new WaitForSeconds(delay);
		StartCoroutine(spawnAnts (delay));
	}
}