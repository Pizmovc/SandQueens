using UnityEngine;
using System.Collections;
using ResourceManager;

public class AntQueen : MonoBehaviour {
	public GameObject ant;
	public Vector3 spawnLocation;
	public float timeBetweenSpawning;

	// Use this for initialization
	void Start () {
		//transform.position = new Vector3 (RM.TerrainMesh.Width/2, RM.TerrainMesh.TerrainHeightFromFloor + transform.localScale.x/2, RM.TerrainMesh.Height/2);
		StartCoroutine (spawnAnts(timeBetweenSpawning));
	}
	
	// Update is called once per frame
	void Update () {

	}

	IEnumerator spawnAnts(float delay){
		Instantiate (ant, spawnLocation, Quaternion.identity);
		yield return new WaitForSeconds(delay);
		StartCoroutine(spawnAnts (delay));
	}
}