using UnityEngine;
using System.Collections;
using ResourceManager;

[RequireComponent(typeof(Terrain))]
[RequireComponent(typeof(TerrainCollider))]

public class GenerateTerrarium : MonoBehaviour {
	// Use this for initialization
	void Start () {
		transform.position = new Vector3 (0, RM.Terrarium.sandBaseHeight, 0);
		TerrainData terrainData = RM.Terrarium.terrainData;

		GetComponent<Terrain> ().terrainData = terrainData;
		//GetComponent<Terrain> ().
		GetComponent<TerrainCollider> ().terrainData = terrainData;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
