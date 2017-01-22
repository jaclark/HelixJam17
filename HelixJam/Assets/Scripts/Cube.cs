using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

	public Material[] cubeMaterials;
	public Material ACube;
	public ParticleSystem DestroyedParticles;
	public string cubeType;

	// Use this for initialization
	void Start () {
		float randScale = Random.Range (1f, 3f);
		transform.localScale += new Vector3 (randScale, randScale, 0.0f);
		int cubeMatIndex = Random.Range (0, 2);
		MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer> ();
		meshRenderer.material = cubeMaterials [cubeMatIndex];
		cubeType = meshRenderer.material.ToString ();
	}
}
