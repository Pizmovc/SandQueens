using UnityEngine;
using System.Collections;
using ResourceManager;

[RequireComponent(typeof(Terrain))]
[RequireComponent(typeof(TerrainCollider))]

public class GenerateTerrarium : MonoBehaviour {
	float scale = 80.0f;


	// Use this for initialization
	void Start () {
		transform.position = new Vector3 (0, RM.Terrarium.sandBaseHeight, 0);
		TerrainData terrainData = new TerrainData();
		terrainData.size = new Vector3(RM.Terrarium.width, RM.Terrarium.height - RM.Terrarium.sandBaseHeight, RM.Terrarium.length);
		terrainData.heightmapResolution = 513;
		terrainData.baseMapResolution = 1024;
		terrainData.SetDetailResolution(1024, 16);
		float[,] heights = new float[terrainData.heightmapWidth,terrainData.heightmapHeight];

		Vector2 randomLocation;
		randomLocation.x = Random.Range(-10000,10000);
		randomLocation.y = Random.Range(-10000,10000);
		Debug.Log (terrainData.heightmapWidth + " x " + terrainData.heightmapWidth);
		for (int x = 0; x < terrainData.heightmapWidth; x++) {
			for (int z = 0; z < terrainData.heightmapWidth; z++) {
				heights[x,z] = Mathf.PerlinNoise(randomLocation.x + (float)x/scale, randomLocation.y + (float)z/scale);
				//Debug.Log(heights[x,z]);
			}
		}
		terrainData.SetHeights (0, 0, heights);
		GetComponent<Terrain> ().terrainData = terrainData;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
