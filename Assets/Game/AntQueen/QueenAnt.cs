using UnityEngine;
using System.Collections;
using ResourceManager;

public class QueenAnt : Ant {
	private int successiveAntNumber = 0;
	public float timeBetweenSpawning;
	public GameObject antPrefab;
	Vector3 startingPosition = new Vector3 ();


	// Use this for initialization
	public override void Start () {
		startingPosition.x = (float)RM.Terrarium.terrainData.size.x / 10;
		startingPosition.y = (float)RM.Terrarium.height;
		startingPosition.z = (float)RM.Terrarium.terrainData.size.z / 2;
		transform.position = startingPosition;
		StartCoroutine (spawnAnts(timeBetweenSpawning));
	}
	
	// Update is called once per frame
	public override void Update () {


	}


	IEnumerator spawnAnts(float delay){
		GameObject ant = (GameObject)Instantiate (antPrefab, transform.position, Quaternion.identity);
		ant.name = "Ant " + successiveAntNumber.ToString();
		successiveAntNumber ++;
        ant.GetComponent<Ant>().AddNode(new Vector3(Random.Range(10, RM.Terrarium.terrainData.size.x - 10), 0, Random.Range(10, RM.Terrarium.terrainData.size.z - 10)), NodeType.food);
        ant.GetComponent<Ant>().AddNode(new Vector3(Random.Range(10, RM.Terrarium.terrainData.size.x - 10), 0, Random.Range(10, RM.Terrarium.terrainData.size.z - 10)), NodeType.food);
        ant.GetComponent<Ant>().AddNode(transform.position, NodeType.antHill);
		yield return new WaitForSeconds(delay);
		transform.Rotate (new Vector3 (0, 0, 0));
		StartCoroutine(spawnAnts (delay));
	}
}