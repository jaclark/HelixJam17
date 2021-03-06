﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
	public MeshRenderer renderer = null;
	public Material[] cubeMaterials;
	public Material ACube;
	public ParticleSystem DestroyedParticlesPrefab;
	public string cubeType;

	public static int nextMatIndex = 0;

	// Use this for initialization
	void Start () {
		float randScale = Random.Range (0.5f, 2.0f);
		transform.localScale += new Vector3 (randScale, randScale, 0.0f);
		int cubeMatIndex = nextMatIndex++;
		renderer.material = cubeMaterials [cubeMatIndex%2];
		cubeType = renderer.material.ToString ();
	}

	private void Update()
	{
		transform.rotation *= Quaternion.AngleAxis (180 * Time.deltaTime, Vector3.forward);
	}

	public void MakeDemParticles()
	{
		Instantiate (DestroyedParticlesPrefab, transform.position, Quaternion.identity);
	}
}
