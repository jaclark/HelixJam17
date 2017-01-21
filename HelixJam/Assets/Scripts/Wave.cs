using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wave : MonoBehaviour 
{
	public Action<Wave> LineCrossed = delegate {};

	private bool _positive = true;

	private void Start()
	{
		GetComponent<Renderer> ().enabled = false;

		_positive = transform.position.x >= 0;
		Debug.Log (_positive);
	}

	public void ApplyDelta(Vector3 delta)
	{
		transform.position += delta * Time.deltaTime;
	}

	private void Update()
	{
		CheckLineCross ();
	}

	private void CheckLineCross()
	{
		if ((transform.position.x > 0 && !_positive) || (transform.position.x < 0 && _positive))
		{
			OnLineCross ();
		}
	}

	private void OnLineCross()
	{
		_positive = !_positive;

		LineCrossed (this);
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "cube") {
			Cube hitCube = (Cube) col.gameObject.GetComponent<Cube>();
			if (hitCube.cubeType[0] == transform.name[0])
				Destroy (hitCube.gameObject);
		}
	}
}
