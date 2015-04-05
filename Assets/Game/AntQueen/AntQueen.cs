using UnityEngine;
using System.Collections;

public class AntQueen : MonoBehaviour {
	public GameObject ant;
	public float timeBetweenSpawning;

	// Use this for initialization
	void Start () {
		StartCoroutine (spawnAnts(timeBetweenSpawning));
	}
	
	// Update is called once per frame
	void Update () {

	}

	IEnumerator spawnAnts(float delay){
		Instantiate (ant);
		yield return new WaitForSeconds(delay);
		StartCoroutine(spawnAnts (delay));
	}
}