using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
	public MeshRenderer renderer = null;
	public Material[] cubeMaterials;
	public Material ACube;
	public ParticleSystem DestroyedParticles;
	public string cubeType;

	// Use this for initialization
	void Start () {
		float randScale = Random.Range (0.5f, 2.0f);
		transform.localScale += new Vector3 (randScale, randScale, 0.0f);
		int cubeMatIndex = Random.Range (0, 2);
		renderer.material = cubeMaterials [cubeMatIndex];
		cubeType = renderer.material.ToString ();
	}

	private void Update()
	{
		transform.rotation *= Quaternion.AngleAxis (180 * Time.deltaTime, Vector3.forward);
	}
}
