using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile {

	private Vector2 positionOnTerrain;
	public float feromoneLevel;
	private bool empty;
	private List<Tile> adjacentTiles= new List<Tile>();

	/// <summary>
	/// Initializes a new instance of the <see cref="Tile"/> class. Sets feromone level to <c>0.0f</c> and <c>empty</c> tag to <c>true</c>.
	/// </summary>
	public Tile(){
		feromoneLevel = 0.0f;
		empty = true;
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="Tile"/> class. Sets feromone level to <c>0.0f</c> and <c>empty</c> tag to <c>true</c>.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public Tile(int x, int y){
		positionOnTerrain.x = x;
		positionOnTerrain.y = y;
		feromoneLevel = 0.0f;
		empty = true;
	}
	/// <summary>
	/// Add the feromone level.
	/// </summary>
	/// <param name="level">Amount added.</param>
	public void AddFeromoneLevel(float level){
		feromoneLevel += level;
	}
	/// <summary>
	/// Gets the feromone level.
	/// </summary>
	/// <returns>The feromone level.</returns>
	public float GetFeromoneLevel(){
		return feromoneLevel;
	}

	/// <summary>
	/// Sets the empty tag.
	/// </summary>
	/// <param name="status">If set to <c>true</c> status.</param>
	public void SetEmpty(bool status){
		empty = status;
	}
	/// <summary>
	/// Determines whether this instance is empty.
	/// </summary>
	/// <returns><c>true</c> if this instance is empty; otherwise, <c>false</c>.</returns>
	public bool IsEmpty(){
		return(empty);
	}
	/// <summary>
	/// Adds the adjacent tile of the current tile.
	/// </summary>
	/// <param name="adjacentTile">Adjacent tile.</param>
	public void AddAdjacentTile(Tile adjacentTile){
		adjacentTiles.Add (adjacentTile);
	}

	/// <summary>
	/// Gets a list of adjacent tiles.
	/// </summary>
	/// <returns>The adjacent tiles list.</returns>
	public List<Tile> GetAdjacentTiles(){
		return adjacentTiles;
	}
	/// <summary>
	/// Gets the terrain coordinates.
	/// </summary>
	/// <returns>The terrain coordinates.</returns>
	public Vector2 GetTerrainCoordinates(){
		return positionOnTerrain;
	}
}
