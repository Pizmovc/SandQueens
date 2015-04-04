﻿using UnityEngine;
using UnityEditor;
using System.Collections;

//Remember, also, that any variables or methods we add to ResourceManager will also need to be declared as static.

namespace ResourceManager {
	public static class RM {
		public static float ScrollSpeed { get { return 80; }}
		public static float MoveSpeed { get { return 70; }}
		public static int ScrollWidth { get { return 30; } }
		public static float MinCameraHeight { get { return 5; } }
		public static float MaxCameraHeight { get { return 50; } }


		public static class TerrainMesh {
			private static Mesh OriginalTerrainMesh;
			public static float BlockSize { get { return 1;}}
			public static int Height { get { return 60; }}
			public static int Width { get { return 100; }}
			//public static Color Color { get { return Color.yellow; }}
			public static Mesh  Mesh{ 
				get {
					if(Resources.Load("terrainMesh") != null){
						OriginalTerrainMesh = Resources.Load("terrainMesh") as Mesh;


					}
					else{
						OriginalTerrainMesh = new Mesh();
						OriginalTerrainMesh.Clear();
						OriginalTerrainMesh.vertices = Vertices;
						OriginalTerrainMesh.triangles = Triangles;
						OriginalTerrainMesh.uv = UV;
						OriginalTerrainMesh.RecalculateNormals();
						OriginalTerrainMesh.RecalculateBounds();
						OriginalTerrainMesh.Optimize();
						AssetDatabase.CreateAsset(OriginalTerrainMesh,"Assets/Resources/terrainMesh.asset");
					}
					return OriginalTerrainMesh;
				}
			}
			public static Vector3[] Vertices {
				get {
					Vector3[] TemporaryVertices = new Vector3[2 * 2 +	//the bottom left and top right corners
					                                          2 * 1	+	//the top left and bottom right corners
					                                          (2 * (Width - 1) + 2 * (Height - 1)) * 3 +	//framework with no corners
					                                          (Width - 1) * (Height - 1) * 6	//the inside part
					                                          ];
					Debug.Log(TemporaryVertices.Length);
					int indexCounter = 0;
					for(int h = 0; h < Height; h++){
						for(int w = 0; w < Width; w++){
							for(int i = 0; i < 2; i++){
								TemporaryVertices[indexCounter++] = new Vector3(w,0,h);
								if(i == 0){
									TemporaryVertices[indexCounter++] = new Vector3(w,0,h + BlockSize);
									TemporaryVertices[indexCounter++] = new Vector3(w + BlockSize,0,h + BlockSize);
								}
								else{
									TemporaryVertices[indexCounter++] = new Vector3(w + BlockSize,0,h + BlockSize);
									TemporaryVertices[indexCounter++] = new Vector3(w + BlockSize,0,h);
								}
							}
						}
					}
					Debug.Log(indexCounter);
					return TemporaryVertices;
				}
			}
			public static Vector2[] UV{
				get {
					Vector2[] TemporaryUVs = new Vector2[OriginalTerrainMesh.vertexCount];

					//Debug.Log();
					int indexCounter = 0;
					for(int h = 0; h < Height; h++){
						for(int w = 0; w < Width; w++){
							for(int i = 0; i < 2; i++){
								TemporaryUVs[indexCounter++] = new Vector2(0,0);
								if(i == 0){
									TemporaryUVs[indexCounter++] = new Vector2(0,1);
									TemporaryUVs[indexCounter++] = new Vector2(1,1);
								}
								else{
									TemporaryUVs[indexCounter++] = new Vector3(1,1);
									TemporaryUVs[indexCounter++] = new Vector3(1,0);
								}
							}
						}
					}
					return(TemporaryUVs);
				}
			}
			public static int[] Triangles{
				get {
					int[] TemporaryTriangles = new int[OriginalTerrainMesh.vertices.Length];
					for(int i = 0; i < OriginalTerrainMesh.vertices.Length; i++){
						TemporaryTriangles[i] = i;
					}
					return TemporaryTriangles;
				}
			}
		}
	}
}