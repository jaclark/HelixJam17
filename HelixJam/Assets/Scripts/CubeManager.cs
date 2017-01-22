using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour {

	public Cube cube = null;
	public Material aCubeMat = null;
	public Material bCubeMat = null;

	public int numCubes = 100;

	private float cubeStartY = 10.0f;

	// Use this for initialization
	void Start () {
		RandomLayout (numCubes);
	}

	public void RandomLayout (int _numCubes)
	{
		float spawnPosY = cubeStartY;
		while (_numCubes > 0)
		{
			float xSpawn = Random.Range (2.0f, 12.0f);
			if (xSpawn > 9.0f)
				xSpawn = 5.0f - xSpawn;
			Vector3 spawnPoint = new Vector3 (xSpawn, spawnPosY + Random.Range(15.0f, 20.0f), 0.0f);
			Instantiate(cube, spawnPoint, Quaternion.identity, null);
			spawnPosY += Random.Range (12.0f, 17.0f);
			_numCubes--;
		}
	}
}
