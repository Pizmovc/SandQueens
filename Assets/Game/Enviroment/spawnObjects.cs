using UnityEngine;
using System.Collections;
using ResourceManager;

public class spawnObjects : MonoBehaviour {
    public GameObject rock;
	// Use this for initialization
	void Start () {
        for (int i = 0; i < 300; i++)
        {
            Vector3 spawnLocation = new Vector3();
            spawnLocation.x = Random.Range(5.0f, RM.Terrarium.width - 5);
            spawnLocation.z = Random.Range(5.0f, RM.Terrarium.length - 5);
            spawnLocation.y = RM.Terrarium.height + 10;

            Instantiate(rock, spawnLocation, Quaternion.identity);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
