using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public AudioSource Melody;
	public AudioSource Beat;
	public Transform Wave;  

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Wave.position.x != 0) Melody.volume = 1 - Mathf.Abs(Wave.position.x) / 15;
		if (Wave.position.x != 0) Beat.volume = 1 - Mathf.Abs (Wave.position.x) / 15;

	}
}
