using System.Collections;
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
	private List<GameObject> objectsInGrid;
	private List<GameObject> oldGrid;

	// Use this for initialization
	void Start () {
		obstacleGrid = new int[gridWidth, gridHeight];
		for (int i = 0; i < gridWidth; i++) {
			for (int j = 0; j < gridHeight; j++) {
				obstacleGrid[i,j] = 0;
			}
		}
		numOpenSpaces = (obstacleGrid.GetLength (0) * obstacleGrid.GetLength (0))/3;
		objectsInGrid = new List<GameObject> ();
		oldGrid = new List<GameObject> ();
		drawNewGrid ();
	}

	void Update() {
		// function that checks playerPos being in current grid and builds the next set
		if (playerPos.position.y > gridStartYPos) {
			//disableObjectsPoint = gridStartYPos;
			drawNewGrid ();
		}
		// function that checks playerPos and gets rid of previous
		if (playerPos.position.y > disableObjectsPoint) {
			//clearOldGrid (oldGrid);
		}
	}

	private void drawNewGrid()
	{
		oldGrid = objectsInGrid;
		gridStartYPos = playerPos.position.y + 40.0f;
		// BuildLineWalls first
		BuildLineWalls (new Vector3(-(float)gridWidth/2, gridStartYPos, 0f));
		// then build outer walls
		//BuildOuterWalls(Random.Range(2,10));
		// then put cubes
		BuildCubes(new Vector3((float)-gridWidth/2, gridStartYPos, 0f));
		// then gravity zones and direciton zones
		putWalls (new Vector3(-(float)gridWidth/2, gridStartYPos, 0f));
	}

	private void clearOldGrid(List<GameObject> _oldGrid)
	{
		foreach (GameObject obj in _oldGrid) 
		{
			obj.SetActive (false);
		}
	}

	private void putWalls (Vector3 gridPos)
	{
		//int numWalls = diffcultyVal * 3;
		float startingY = gridPos.y;
		for (int i = 0; i < obstacleGrid.GetLength(0); i++) {
			for (int j = 0; j < obstacleGrid.GetLength (1); j++) {
				//Wall freshWall = Instantiate (wall, gridPos, Quaternion.identity, null);
				gridPos.Set (gridPos.x, gridPos.y + 1, gridPos.z);
			}
			gridPos.Set (gridPos.x + 1, startingY, gridPos.z);
		}
	}

	// puts line walls in the grid
	// returns the number of grid spaces used
	// NOTE: All Line walls should be lineWallWidth thick
	private int BuildLineWalls (Vector3 gridPos)
	{
		float startingY = Random.Range (gridPos.y, gridPos.y + (float)gridHeight);
		int lineWallLength = Random.Range (2, gridHeight / 3 + 1);
		int lineWallWidth = 2;

		gridPos.Set(0f, gridPos.y, gridPos.z); // set gridPos in the middle

		for (int j = 0; j < obstacleGrid.GetLength (1); j++) {
			while (lineWallLength > 0) {
				Wall freshWall = Instantiate (wall, gridPos, Quaternion.identity, null);
				objectsInGrid.Add (freshWall.gameObject);
				freshWall.transform.localScale = new Vector3 ((float)lineWallWidth, 1f, 1f);
				if ((int)gridPos.y < obstacleGrid.GetLength(1)) {
					obstacleGrid [(int)gridPos.x, (int)gridPos.y] += 1;
					obstacleGrid [(int)gridPos.x+1, (int)gridPos.y] += 1;
				}
				gridPos.Set (gridPos.x, gridPos.y + 1, gridPos.z); // build upward
				lineWallLength--;
			}
		}
		return lineWallLength * lineWallWidth;
	}

	// puts random walls
	public void BuildOuterWalls (int _numWalls)
	{
//		float ySpawn = Random.Range (12.0f, 15.0f);
//		while (_numWalls > 0) 
//		{
//			ySpawn += Random.Range (15.0f, 25.0f);
//			Vector3 spawnPoint = new Vector3 (0f, ySpawn, 0f);
//			Wall freshWall = Instantiate (wall, spawnPoint, Quaternion.identity, null);
//			float wallSize = Random.Range (1.0f, 4.0f);
//			freshWall.transform.localScale = new Vector3 (wallSize, 1f, 1f);
//			float wallXPos = Random.Range (-freshWall.transform.localScale.x, freshWall.transform.localScale.x);
//			freshWall.transform.Translate(new Vector3(wallXPos,0f,0f));
//			_numWalls--;
//		}
	}

	// puts cubes in each grid
	public void BuildCubes (Vector3 gridPos)
	{
		int numCubes = Random.Range (2, 8);
		Debug.Log (gridWidth / 4 * 3);
		while (numCubes > 0) 
		{
			float xSpawn = Random.Range (-(gridWidth / 8 * 3), gridWidth / 8 * 3);
			if (xSpawn > -(gridWidth / 6) && xSpawn < gridWidth / 6 ) {
				xSpawn += xSpawn;
			}
			float spawnY = Random.Range (gridPos.y, gridPos.y + (float)gridHeight);
			Vector3 spawnPoint = new Vector3 (xSpawn, spawnY);
			Cube freshCube = Instantiate (cube, spawnPoint, Quaternion.identity, null);
			objectsInGrid.Add (freshCube.gameObject);
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
