using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour {

	public Wall wall = null;
	public int numWalls = 100;

	// Use this for initialization
	void Start () {
		RandomWallLayout (numWalls);
	}

	public void RandomWallLayout (int _numWalls)
	{
		float ySpawn = Random.Range (12.0f, 15.0f);
		while (_numWalls > 0) 
		{
			ySpawn += Random.Range (15.0f, 25.0f);
			Vector3 spawnPoint = new Vector3 (0f, ySpawn, 0f);
			Wall freshWall = Instantiate (wall, spawnPoint, Quaternion.identity, null);
			float wallSize = Random.Range (1.0f, 4.0f);
			freshWall.transform.localScale = new Vector3 (wallSize, 1f, 1f);
			float wallXPos = Random.Range (-freshWall.transform.localScale.x, freshWall.transform.localScale.x);
			freshWall.transform.Translate(new Vector3(wallXPos,0f,0f));
			_numWalls--;
		}
	}
}
