using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour {

	public Cube cube = null;
	public Material aCubeMat = null;
	public Material bCubeMat = null;

	public int numCubes = 40;

	private float cubeStartY = 10.0f;

	// Use this for initialization
	void Start () {
		RandomLayout (numCubes);
	}

	public void SpawnNewCube (Vector3 oldPos)
	{
		Debug.Log ("SpawnNewCube");
		//Vector3 spawnPoint = new Vector3 (Random.Range(-4.5f,4.5f), oldPos.y + Random.Range(12.0f, 17.0f), oldPos.z);
		//Instantiate(cube, spawnPoint, Quaternion.identity, null);
	}

	public void RandomLayout (int _numCubes)
	{
		float spawnPosY = cubeStartY;
		while (_numCubes > 0)
		{
			Vector3 spawnPoint = new Vector3 (Random.Range(-6.0f, 6.0f), spawnPosY + Random.Range(12.0f, 17.0f), 0.0f);
			Instantiate(cube, spawnPoint, Quaternion.identity, null);
			spawnPosY += Random.Range (12.0f, 17.0f);
			_numCubes--;
		}
	}
}
