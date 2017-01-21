using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveMaster : MonoBehaviour 
{
	public List<Wave> waves = null;
	public BasicCamera basicCamera = null;

	public float speed = 0.0f;
	[Range(0.0f, 1.0f)]
	public float gravity = 0.0f;
	[Range(0.0f, 1.0f)]
	public float boostGravity = 0.0f;

	public float boost = 0.0f;
	[Range(0.0f, 1.0f)]
	public float boostDecel = 0.0f;

	public float jump = 0.0f;
	public int jumpsAllowed = 0;
	private int _jumps = 0;

	public Text debugText = null;
	public Text jumpText = null;

	private void Start()
	{
		for (int i = 0; i < waves.Count; ++i)
		{
			waves [i].Init (jump, boost, gravity, boostDecel, speed, boostGravity);
			waves [i].LineCrossed += OnLineCrossed;
		}

		basicCamera.Init (waves);

		jumpText.text = _jumps.ToString ();
	}

	private void Update()
	{
		if ((Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown(KeyCode.D)) && _jumps > 0)
		{
			Jump ();
		}

		if (Input.GetKeyDown (KeyCode.W))
		{
			Boost ();
		}
	}

	private void Jump()
	{
		_jumps--;
		_jumps = Mathf.Clamp (_jumps, 0, jumpsAllowed);
		jumpText.text = _jumps.ToString ();

		for (int i = 0; i < waves.Count; ++i)
		{
			waves [i].Jump ();
		}
	}

	private void Boost()
	{
		_jumps--;
		_jumps = Mathf.Clamp (_jumps, 0, jumpsAllowed);
		jumpText.text = _jumps.ToString ();

		for (int i = 0; i < waves.Count; ++i)
		{
			waves [i].BoostSpeed ();
		}
	}

	private void OnLineCrossed(Wave wave)
	{
		_jumps = jumpsAllowed;
		jumpText.text = _jumps.ToString ();
	}

	private void OnDestroy()
	{
		for (int i = 0; i < waves.Count; ++i)
		{
			waves [i].LineCrossed -= OnLineCrossed;
		}
	}
}
