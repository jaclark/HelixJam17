using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterLine : MonoBehaviour 
{
	private Renderer _renderer = null;

	private void Start()
	{
		_renderer = GetComponent<Renderer> ();
	}

	public void SetColors(Color a, Color b)
	{
		_renderer.material.SetColor ("_ColorA", a);
		_renderer.material.SetColor ("_ColorB", b);
	}

	public void SetPositions(float a, float b)
	{
		_renderer.material.SetFloat ("_XPosA", a);
		_renderer.material.SetFloat ("_XPosB", b);
	}
}
