﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMaster : MonoBehaviour {

	public DirectionZone dirZone = null;
	public NoGravZone noGravZone = null;
	public Cube cube = null;
	public Wall wall = null;
	public Transform playerPos = null;
	public int[,] obstacleGrid;
	public int gridWidth = 1;
	public int gridHeight = 1;

	private int numOpenSpaces; // keeps track of how many more spaces we can fill in obstacle grid
	private float gridStartYPos;
	private float disableObjectsPoint;
	private List<GameObject> nextGrid;
	private List<GameObject> currGrid;
	private List<GameObject> prevGrid;

	// Use this for initialization
	void Start () {
		obstacleGrid = new int[gridWidth, gridHeight];
		for (int i = 0; i < gridWidth; i++) {
			for (int j = 0; j < gridHeight; j++) {
				obstacleGrid[i,j] = 0;
			}
		}
		numOpenSpaces = (obstacleGrid.GetLength (0) * obstacleGrid.GetLength (0))/3;
		nextGrid = new List<GameObject> ();
		currGrid = new List<GameObject> ();
		prevGrid = new List<GameObject> ();
		drawNewGrid ();
	}

	void Update() {
		// function that checks playerPos being in current grid and builds the next set
		if (playerPos.position.y > gridStartYPos) {
			drawNewGrid ();
			//clearOldGrid (prevGrid);
		}
		// function that checks playerPos and gets rid of previous
//		if (playerPos.position.y > disableObjectsPoint + 20f) {
//			clearOldGrid (prevGrid);
//		}

		if (playerPos.position.y > disableObjectsPoint) {
			clearOldGrid (prevGrid);
		}
	}

	private void drawNewGrid()
	{
		disableObjectsPoint = gridStartYPos - 50f;
		// update grid references
		prevGrid = currGrid;
		currGrid = nextGrid;
		nextGrid = nextGrid = new List<GameObject> ();
		//zeroOutOldGrid ();
		gridStartYPos = playerPos.position.y + gridHeight;
		// BuildLineWalls first
		BuildLineWalls (new Vector3(-(float)gridWidth/2, gridStartYPos, 0f));
		// then build outer walls
		BuildOuterWalls(new Vector3((float)-gridWidth/2, gridStartYPos, 0f));
		// then put cubes
		BuildCubes(new Vector3((float)-gridWidth/2, gridStartYPos, 0f));
		// then gravity zones and direciton zones
		//putWalls (new Vector3(-(float)gridWidth/2, gridStartYPos, 0f));
		//printObstacleGrid ();
	}

	private void clearOldGrid(List<GameObject> _prevGrid)
	{
		foreach (GameObject obj in _prevGrid) 
		{
			obj.SetActive (false);
		}
	}

	private void zeroOutOldGrid ()
	{
		for (int i = 0; i < gridWidth; i++) {
			for (int j = 0; j < gridHeight; j++) {
				obstacleGrid [i, j] = 0;
			}
		}
	}

//	private void putWalls (Vector3 gridPos)
//	{
//		//int numWalls = diffcultyVal * 3;
//		float startingY = gridPos.y;
//		for (int i = 0; i < obstacleGrid.GetLength(0); i++) {
//			for (int j = 0; j < obstacleGrid.GetLength (1); j++) {
//				//Wall freshWall = Instantiate (wall, gridPos, Quaternion.identity, null);
//				gridPos.Set (gridPos.x, gridPos.y + 1, gridPos.z);
//			}
//			gridPos.Set (gridPos.x + 1, startingY, gridPos.z);
//		}
//	}

	// puts line walls in the grid
	// returns the number of grid spaces used
	// NOTE: All Line walls should be lineWallWidth thick
	private int BuildLineWalls (Vector3 gridPos)
	{
		float startingY = Random.Range (gridPos.y, gridPos.y + (float)gridHeight);
		int lineWallLength = Random.Range (2, gridHeight / 3 + 1);
		int lineWallWidth = 2;

		gridPos.Set(0f, gridPos.y, gridPos.z); // set gridPos in the middle

		//for (int j = 0; j < obstacleGrid.GetLength (1); j++) {
			//while (lineWallLength > 0) {
				Wall freshWall = Instantiate (wall, gridPos, Quaternion.identity, null);
				nextGrid.Add (freshWall.gameObject);
				freshWall.transform.localScale = new Vector3 ((float)lineWallWidth, lineWallLength, 1f);
				freshWall.rectTransform.localScale = new Vector3 (1f / lineWallWidth, 1f/lineWallLength, 1f);
				freshWall.rectTransform.sizeDelta = new Vector2 (freshWall.rectTransform.sizeDelta.x * lineWallWidth, freshWall.rectTransform.sizeDelta.y*lineWallLength);
				if ((int)gridPos.y < obstacleGrid.GetLength(1)) {
					Debug.Log ("HERE");
					obstacleGrid [(int)gridPos.x, (int)gridPos.y] += 1;
					obstacleGrid [(int)gridPos.x+1, (int)gridPos.y] += 1;
				}
				gridPos.Set (gridPos.x, gridPos.y + 1, gridPos.z); // build upward
				lineWallLength--;
			//}
		//}
		return lineWallLength * lineWallWidth;
	}

	// puts random walls
	public void BuildOuterWalls (Vector3 gridPos)
	{
//		float ySpawn = Random.Range (12.0f, 15.0f);
		int numWalls = Random.Range (1,5);
		while (numWalls > 0) 
		{
			float xSpawn = Random.Range (-gridWidth / 2 + 2, gridWidth / 2 - 5);
			if (xSpawn > -(gridWidth / 3) && xSpawn < gridWidth / 3 ) {
				xSpawn += xSpawn;
			}
			float spawnY = Random.Range (gridPos.y, gridPos.y + (float)gridHeight);
			Vector3 spawnPoint = new Vector3 (xSpawn, spawnY);
			Wall freshWall = Instantiate (wall, spawnPoint, Quaternion.identity, null);
			float xScale = Random.Range (1f, 4f);
			float yScale = Random.Range (1f, 4f);
			freshWall.transform.localScale = new Vector3 (xScale, yScale, 1f);
			freshWall.rectTransform.localScale = new Vector3 (1f / xScale, 1f / yScale, 1f);
			freshWall.rectTransform.sizeDelta = new Vector2 (freshWall.rectTransform.sizeDelta.x * xScale, freshWall.rectTransform.sizeDelta.y * yScale);
			nextGrid.Add (freshWall.gameObject);
			if ((int)gridPos.y < obstacleGrid.GetLength(1)) {
				for (int i = 0; i < freshWall.transform.localScale.x; i++) {
					for (int j = 0; j < freshWall.transform.localScale.y; j++) {
						Debug.Log ("HERE");
						obstacleGrid [(int)gridPos.x, (int)gridPos.y] += 1;
						obstacleGrid [(int)gridPos.x + 1, (int)gridPos.y] += 1;
					}
				}
			}
			// should update obstacle grid
			numWalls--;
		}
	}

	// puts cubes in each grid
	public void BuildCubes (Vector3 gridPos)
	{
		int numCubes = Random.Range (2, 5);
		while (numCubes > 0) 
		{
			float xSpawn = Random.Range (-gridWidth / 2 + 3, gridWidth / 2 - 3);
			if (xSpawn > -(gridWidth / 6) && xSpawn < gridWidth / 6 ) {
				xSpawn += xSpawn;
			}
			float spawnY = Random.Range (gridPos.y, gridPos.y + (float)gridHeight);
			Vector3 spawnPoint = new Vector3 (xSpawn, spawnY);
			Cube freshCube = Instantiate (cube, spawnPoint, Quaternion.identity, null);
			nextGrid.Add (freshCube.gameObject);
			// update Obstacle Grid
			if ((int)gridPos.y < obstacleGrid.GetLength(1)) {
				for (int i = 0; i < freshCube.transform.localScale.x; i++) {
					for (int j = 0; j < freshCube.transform.localScale.y; j++) {
						Debug.Log ("HERE");
						obstacleGrid [(int)gridPos.x, (int)gridPos.y] += 1;
						obstacleGrid [(int)gridPos.x + 1, (int)gridPos.y] += 1;
					}
				}
			}
			numCubes--;
		}
	}

	private void printObstacleGrid(){
		string obstacleGridRow = "";
		for (int i = 0; i < gridWidth; i++) {
			for (int j = 0; j < gridHeight; j++) {
				obstacleGridRow += (obstacleGrid [i, j]).ToString() + " ";
			}
			Debug.Log(obstacleGridRow);
		}
	}
}
