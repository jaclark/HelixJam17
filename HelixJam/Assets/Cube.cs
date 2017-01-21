using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

	public Material[] cubeMaterials;
	public string cubeType;

	// Use this for initialization
	void Start () {
		float randScale = Random.Range (0.5f, 3.0f);
		transform.localScale += new Vector3 (randScale, randScale, 0.0f);
		int cubeMatIndex = Random.Range (0, 2);
		MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer> ();
		meshRenderer.material = cubeMaterials [cubeMatIndex];
		cubeType = meshRenderer.material.ToString ();
	}
}
