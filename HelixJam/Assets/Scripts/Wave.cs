using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour 
{
	public float upSpeed = 0.0f;
	public float frequency = 0.0f;
	public float amplitude = 0.0f;
	public bool reverse = false;

	private void Update()
	{
		Vector3 delta = Vector3.zero;

		delta.y = upSpeed;

		if (reverse)
		{
			delta.x = Mathf.Sin (Time.time * frequency) * amplitude;
		}
		else
		{
			delta.x = - Mathf.Sin(Time.time * frequency) * amplitude;
		}

		transform.position += delta * Time.deltaTime;
	}
}
