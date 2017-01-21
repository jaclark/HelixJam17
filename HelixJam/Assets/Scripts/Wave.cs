using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour 
{
	public float speed = 0.0f;
	[Range(0.0f, 1.0f)]
	public float gravity = 0.0f;
	[Range(0.0f, 1.0f)]
	public float influence = 0.0f;
	[Range(0.0f, 1.0f)]
	public float influenceAccel = 0.0f;

	public float jump = 0.0f;

	private float _xMove = 0.0f;


	private void Start()
	{
		GetComponent<Renderer> ().enabled = false;
	}

	private void Update()
	{
		Vector3 delta = Vector3.zero;

		delta.y = speed;
		if (Input.GetKeyDown (KeyCode.Space))
		{
			if (transform.position.x > 0)
			{
				float aboveZeroX = _xMove;
				if (aboveZeroX < 0)
				{
					aboveZeroX = 0;
				}
				_xMove = aboveZeroX + jump;
			}
			else
			{
				float belowZeroX = _xMove;
				if (belowZeroX > 0)
				{
					belowZeroX = 0;
				}
				_xMove = belowZeroX - jump;
			}
		}
		else
		{
			_xMove += -transform.position.x * (gravity);
		}
		delta.x = _xMove;

		transform.position += delta * Time.deltaTime;
	}
}
