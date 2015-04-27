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
		GameObject ant = (GameObject)Instantiate (antPrefab, transform.position, /*  
            new Vector3(transform.position.x + Mathf.Pow(-1,Random.Range(0,3)) * Random.Range(1.0f,3.0f),
                        transform.position.y + 1,
                        transform.position.z + Mathf.Pow(-1, Random.Range(0, 3)) * Random.Range(1.0f, 3.0f)), */
            Quaternion.identity);
		ant.name = "Ant " + successiveAntNumber.ToString();
		successiveAntNumber ++;
		ant.GetComponent<Ant>().AddDestination(new Vector3(Random.Range(10,RM.Terrarium.terrainData.size.x-10),0, Random.Range(10,RM.Terrarium.terrainData.size.z-10)));
        ant.GetComponent<Ant>().AddDestination(transform.position);
        ant.GetComponent<Ant>().SetHomeCoordinates(transform.position);
		yield return new WaitForSeconds(delay);
		transform.Rotate (new Vector3 (0, 0, 0));
		StartCoroutine(spawnAnts (delay));
	}
}