using UnityEngine;
using System.Collections;

public class spawnAnts : MonoBehaviour {
	public GameObject antPrefab;
	public float spawnDelayTime;
	public float foodAmount;


	// Use this for initialization
	void Start () {
		StartCoroutine(waitAndSpawn (spawnDelayTime));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator waitAndSpawn(float delayTime){
		Debug.Log ("Delay in seconds before spawning: "+ delayTime);
		yield return new WaitForSeconds(delayTime);
		if (foodAmount > 0) {
			foodAmount--;
			Instantiate (antPrefab, new Vector2 (Random.Range (-5.0f, 5.0f), Random.Range (-5.0f, 5.0f)), Quaternion.identity);
		}
		StartCoroutine(waitAndSpawn (spawnDelayTime + Random.Range(-1.0f,1.0f)));
	}
}
