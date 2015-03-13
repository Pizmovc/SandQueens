using UnityEngine;
using System.Collections;

public class spawnAnts : MonoBehaviour {
	public GameObject antPrefab;
	public float spawnDelayTime;

	// Use this for initialization
	void Start () {
		Instantiate (antPrefab, new Vector2(1,1), Quaternion.identity);
		StartCoroutine(waitAndSpawn (spawnDelayTime));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator waitAndSpawn(float delayTime){
		Instantiate (antPrefab, new Vector2(Random.Range(-5.0f,5.0f),Random.Range(-5.0f,5.0f)), Quaternion.identity);
		yield return new WaitForSeconds(delayTime);
		StartCoroutine(waitAndSpawn (delayTime));
	}
}
