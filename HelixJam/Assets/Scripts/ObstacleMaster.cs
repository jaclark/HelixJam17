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
		}

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
		gridStartYPos = playerPos.position.y + gridHeight + 10f;
		Vector3 gridStart = new Vector3 ((float)-gridWidth / 2, gridStartYPos, 0f);
		// BuildLineWalls first
		BuildLineWalls (gridStart);
		// then build outer walls
		BuildOuterWalls(gridStart);
		// then put cubes
		BuildCubes(gridStart);
		// then gravity zones and direciton zones
		BuildAllZones(gridStart);
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
		int numWalls = Random.Range (2,7);
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
		int numCubes = Random.Range (2, 6);
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

	public void BuildAllZones (Vector3 gridPos)
	{
		float bottomZoneValue = Random.Range (0f, 2f);
		float middleZoneValue = Random.Range (0f, 2f);
		float topZoneValue = Random.Range (0f, 2f);
		float[] gridSpots = { bottomZoneValue, middleZoneValue, topZoneValue };
		//gridgridPos.y + gridHeight / 3 * i

		for (int i = 0; i < gridSpots.GetLength (0); i++) {
			Debug.Log ("gridPos first " + gridPos);
			gridPos.Set (gridPos.x, gridPos.y + gridHeight / 3, gridPos.z);
			Debug.Log ("gridPos second " + gridPos);
			if (gridSpots [i] > 0.75f && gridSpots [i] < 1.5f)
				BuildNoGravZones (gridPos);
			else if (gridSpots [i] >= 1.5f && gridSpots [i] <= 2f)
				BuildDirectionZones (gridPos);
		}
	}

	public void BuildNoGravZones (Vector3 gridPos) 
	{
		float xSpread = Random.Range (4.0f, 10.0f);
		float xScale = Random.Range (5.0f, 8.0f);
		float yScale = Random.Range (2.0f, 9.0f);

		float spawnYPos = gridPos.y;

		// check/make that its not touching line/ linewall
		if (-xSpread + xScale <= -1f)
			xSpread = xSpread - 1f;

		Vector3 leftPos = new Vector3 (-xSpread, spawnYPos, 0f);
		NoGravZone leftZone = Instantiate (noGravZone, leftPos, Quaternion.identity, null);
		nextGrid.Add (leftZone.gameObject);
		leftZone.transform.localScale = new Vector3 (xScale, yScale, 1f);
		leftZone.rectTransform.localScale = new Vector3 (1 / xScale, 1 / yScale, 1);
		leftZone.rectTransform.sizeDelta = new Vector2 (leftZone.rectTransform.sizeDelta.x * xScale, leftZone.rectTransform.sizeDelta.y * yScale);

		Vector3 rightPos = new Vector3 (xSpread, spawnYPos, 0f);
		NoGravZone rightZone = Instantiate (noGravZone, rightPos, Quaternion.identity, null);
		nextGrid.Add (rightZone.gameObject);
		rightZone.transform.localScale = new Vector3 (xScale, yScale, 1f);
		rightZone.rectTransform.localScale = new Vector3 (1 / xScale, 1 / yScale, 1);
		rightZone.rectTransform.sizeDelta = new Vector2 (rightZone.rectTransform.sizeDelta.x * xScale, rightZone.rectTransform.sizeDelta.y * yScale);

	}

	public void BuildDirectionZones (Vector3 gridPos)
	{
		float xSpread = Random.Range (3.0f, 10.0f);
		float xScale = Random.Range (3.0f, 6.0f);
		float yScale = Random.Range (2.0f, 10.0f);
		float xDir = Random.Range (-1f, 1f);
		float yDir = Random.Range (0, 1.0f);

		DirectionZone leftZone = Instantiate (dirZone, Vector3.up * -1000, Quaternion.identity, null);
		nextGrid.Add (leftZone.gameObject);
		leftZone.SetReverse (false);
		leftZone.SetPosition (new Vector3 (-xSpread, gridPos.y, 0f));
		leftZone.SetDirection (new Vector3 (xDir, yDir, 0));
		leftZone.SetScale (new Vector3 (xScale, yScale, 1f));

		DirectionZone rightZone = Instantiate (dirZone, Vector3.up * -1000, Quaternion.identity, null);
		nextGrid.Add (rightZone.gameObject);
		rightZone.SetReverse (true);
		rightZone.SetPosition (new Vector3 (xSpread, gridPos.y, 0f));
		rightZone.SetDirection (new Vector2 (xDir, yDir));
		rightZone.SetScale (new Vector3 (xScale, yScale, 1f));
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
