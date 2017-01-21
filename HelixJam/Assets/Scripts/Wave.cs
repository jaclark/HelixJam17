using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour 
{
	public float speed = 0.0f;
	[Range(0.1f,1.0f)]
	public float gravity = 0.0f;

	[Range(0.1f, 1.0f)]
	public float maxInfluence = 0.0f;
	public float influenceAcceleration = 0.0f;
	public float influenceDeceleration = 0.0f;

	private float _influence = 0.0f;

	private float _xMove = 0.0f;

	private void Update()
	{
		Vector3 delta = Vector3.zero;

		delta.y = speed;

		if (Input.GetKey (KeyCode.Space))
		{
			_influence += influenceAcceleration;
		}
		else
		{
			_influence -= influenceDeceleration;
		}
		_influence = Mathf.Clamp (_influence, 0, maxInfluence);

		_xMove += -transform.position.x * (gravity + _influence);
		delta.x = _xMove;

		transform.position += delta * Time.deltaTime;
	}
}
