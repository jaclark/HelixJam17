using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

	public Material[] cubeMaterials;
	public string cubeType;

	// Use this for initialization
	void Start () {
		int cubeMatIndex = Random.Range (0, 2);
		MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer> ();
		meshRenderer.material = cubeMaterials [cubeMatIndex];
		cubeType = meshRenderer.material.ToString ();
	}
}
