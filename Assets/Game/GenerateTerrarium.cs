﻿using UnityEngine;
using System.Collections;
using ResourceManager;

public class GenerateTerrarium : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Mesh mesh = RM.TerrainMesh.Mesh;
		GetComponent<MeshFilter>().mesh = mesh;


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
