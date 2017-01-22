using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundWaver : MonoBehaviour 
{
	public Color[] colors = null;
	public TrailRenderer trail = null;
	public float fallSpeed = 0.0f;
	public float sininess = 0.0f;
	public float lifetime = 0.0f;

	private float _timer = 0f;

	private void Start()
	{
		int randColor = Random.Range (0, colors.Length);
		trail.material.color = colors [randColor];
	}

	private void Update()
	{
		transform.position = new Vector3 (Mathf.Sin (Time.time) * sininess, transform.position.y - fallSpeed * Time.deltaTime, 0);

		_timer += Time.deltaTime;
		if (_timer > lifetime)
		{
			gameObject.SetActive (false);
		}
	}
}
