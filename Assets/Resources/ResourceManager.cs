using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

namespace ResourceManager {
	public static class RM {
        public static int RandomSign { get { return (Random.value < .5f) ? 1 : -1; } }

		public static class Camera{
			public static float scrollSpeed { get { return 100; }}
			public static float moveSpeed { get { return 60; }}
			public static int scrollWidth { get { return 30; } }
			public static float minCameraHeight { get { return RM.Terrarium.height; } }
			public static float maxCameraHeight { get { return 4*RM.Terrarium.height; } }
		}
        public static class AntSettings
        {
            public static float nodeDistance { get { return (Random.Range(2.0f,5.0f)); } }
            public static float nodeSpreadAngle { get { return (RM.RandomSign * Random.Range(10.0f, 45.0f)); } } 
        }
		public static class Terrarium{
			public static int[] GetTerrainCoordinates(Vector3 worldCoordinates){
				float[] terrainCoordinates = new float[2];
				int[] terrainCoordinatesInt = new int[2];
				terrainCoordinates[0] = worldCoordinates.x / terrainData.size.x;
				terrainCoordinates[1] = worldCoordinates.z / terrainData.size.z;

				if (terrainCoordinates [0] > 1 || terrainCoordinates [1] > 1)
					Debug.LogWarning ("GetTerrainCoordinates is not working correctly. terrainCoordinates: " + terrainCoordinates);
				else {
					terrainCoordinatesInt[0] = (int)(terrainCoordinates[0] * terrainData.heightmapWidth);
					terrainCoordinatesInt[1] = (int)(terrainCoordinates[1] * terrainData.heightmapHeight);
				}
				Debug.Log (terrainCoordinatesInt[0] + " x " + terrainCoordinatesInt[1] );
				return(terrainCoordinatesInt);
			}

			private static bool generateNewTerrain { get { return false; }}
			public static float width { get { return 100.0f; }}
			public static float length { get { return width; }}
			public static float height { get { return 20.0f; }}
			public static float sandBaseHeight { get { return 10.0f; }}
			private static int heightmapResolution { get { return 256 + 1; }}
			private static int baseMapResolution { get { return 1024; }}
			private static Vector2 detailResolution { get { return new Vector2(1024, 16); }}
			private static int aplhaMapResolution { get { return heightmapResolution; }}
			private static float scale { get { return 80.0f; }}
			private static SplatPrototype[] splatTextures { get { 
					SplatPrototype[] tempSplat = new SplatPrototype[2];

					Texture2D sand = null;
					if(Resources.Load("Sand") != null)
						sand = Resources.Load("Sand") as Texture2D;
					else
						Debug.LogWarning("Sand texture is missing!");

					Texture2D sandNormal = null;
					if(Resources.Load("Sand_normal") != null)
						sandNormal = Resources.Load("Sand_normal") as Texture2D;
					else
						Debug.LogWarning("Sand_normal texture is missing!");

					tempSplat[0] = new SplatPrototype();
					tempSplat[0].texture = sand;
					tempSplat[0].normalMap = sandNormal;
					tempSplat[0].tileSize = new Vector2(15,15);
					tempSplat[0].tileOffset = new Vector2(0,0);

					Texture2D dirtAndGravel = null;
					if(Resources.Load("Dirt_and_gravel") != null)
						dirtAndGravel = Resources.Load("Dirt_and_gravel") as Texture2D;
					else
						Debug.LogWarning("Dirt_and_gravel texture is missing!");

					Texture2D dirtAndGravelNormal = null;
					if(Resources.Load("Dirt_and_gravel_normal") != null)
						dirtAndGravelNormal = Resources.Load("Dirt_and_gravel_normal") as Texture2D;
					else
						Debug.LogWarning("Dirt_and_gravel_normal texture is missing!");

					tempSplat[1] = new SplatPrototype();
					tempSplat[1].texture = dirtAndGravel;
					tempSplat[1].normalMap = dirtAndGravelNormal;
					tempSplat[1].tileSize = new Vector2(15,15);
					tempSplat[1].tileOffset = new Vector2(0,0);

					return tempSplat; 
				}
			}
//			public static SplatPrototype[] splatTextures { get { 
//					SplatPrototype[] tempSplatTextures = new SplatPrototype[4](AssetDatabase.ImportAsset("Game/Ground Textures/"));
//			}}
			private static bool isTerrainLoaded;
			private static TerrainData _loadedTerrainData;
			private static TerrainData loadedTerrainData { 
				get {
					if(isTerrainLoaded)
						return _loadedTerrainData;
					else{
						Debug.LogWarning("Returning NULL loadedTerrainData.");
						return(null);
					}
				}
				set {
					_loadedTerrainData = value;
					isTerrainLoaded = true;
				}
			}
			/// <summary>
			/// Gets the terrain data either from memory if its loaded, from disk if it isnt, or generates a new one of neither of those are available.
			/// </summary>
			/// <value>The terrain data.</value>
			public static TerrainData terrainData {
				get {
					TerrainData terrainData = new TerrainData();
					if(isTerrainLoaded){
						//Debug.Log("terrainData is already loaded, returning it!");
						return(loadedTerrainData);
					}
					else if(Resources.Load("terrainData") != null && !generateNewTerrain){
						terrainData = Resources.Load("terrainData") as TerrainData;
						Debug.Log("Read terrainData from disk!");
					}
					else{
						Debug.Log("No terrainData found on disk, generating a new random one...");

						terrainData.heightmapResolution = heightmapResolution;
						terrainData.size = new Vector3(width, height - sandBaseHeight, length);
						terrainData.baseMapResolution = baseMapResolution;
						terrainData.SetDetailResolution((int)detailResolution.x, (int)detailResolution.y);
						terrainData.alphamapResolution = aplhaMapResolution;

						Debug.Log ("Terrain size: " + terrainData.size); //Correct terrain size

						float[,] heights = new float[terrainData.heightmapWidth,terrainData.heightmapHeight];
						Vector2 randomLocation;	//for perlin noise
						randomLocation.x = Random.Range(-10000,10000);	//that is hopefully enough
						randomLocation.y = Random.Range(-10000,10000);
						//Debug.Log (terrainData.heightmapWidth + " x " + terrainData.heightmapWidth);
						for (int x = 0; x < terrainData.heightmapWidth; x++) {
							for (int z = 0; z < terrainData.heightmapWidth; z++) {
								heights[x,z] = Mathf.PerlinNoise(randomLocation.x + (float)x/scale, randomLocation.y + (float)z/scale);
								//Debug.Log(heights[x,z]);
							}
						}

						terrainData.SetHeights (0, 0, heights);

						terrainData.splatPrototypes = splatTextures;
						/*
						Debug.Log ("Alpha layers: " + terrainData.alphamapLayers);
						// Splatmap data is stored internally as a 3d array of floats, so declare a new empty array ready for your custom splatmap data:
						float[, ,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

						for (int y = 0; y < terrainData.alphamapHeight; y++){
							for (int x = 0; x < terrainData.alphamapWidth; x++){
								// Get the normalized terrain coordinate that
								// corresponds to the the point.
								float normX = x * 1.0f / (terrainData.alphamapWidth - 1);
								float normY = y * 1.0f / (terrainData.alphamapHeight - 1);
								Debug.Log(normX +" x "+normY);
								// Get the steepness value at the normalized coordinate.
								float angle = terrainData.GetSteepness(normX, normY);
								
								// Steepness is given as an angle, 0..90 degrees. Divide
								// by 90 to get an alpha blending value in the range 0..1.
								float frac = angle / 90.0f;
								splatmapData[x, y, 0] = frac;
								splatmapData[x, y, 1] = 1 - frac;
								//Debug.Log(splatmapData[x, y, 0]);
							}
						}
						
						// Finally assign the new splatmap to the terrainData:
						terrainData.SetAlphamaps(0, 0, splatmapData);
						*/
						//terrainData.RefreshPrototypes();
						//Flush();
                        #if UNITY_EDITOR
                            AssetDatabase.CreateAsset(terrainData, "Assets/Resources/terrainData.asset");
                        #endif
						//terrainData.splatPrototypes[] = AssetDatabase.ImportAsset("Game/Ground Textures/")
					}
					loadedTerrainData = terrainData;
					return(terrainData);
				}
			}
			
		}
		/*
		public static class Grid{
			public static Tile[ , ] grid { 
				get {
					Debug.Log (Terrarium.terrainData.heightmapWidth +" x " + Terrarium.terrainData.heightmapWidth);
					Tile[ , ] tempGrid = new Tile[Terrarium.terrainData.heightmapWidth, Terrarium.terrainData.heightmapWidth];
					for(int x = 0; x < tempGrid.GetLength(0); x++){
						for(int z = 0; z < tempGrid.GetLength(1); z++){
							tempGrid[x,z] = new Tile(x,z);
						}
						//Debug.Log("X: " + x);
					}
					return tempGrid; 
				}
			}
		}
		*/
	}
}