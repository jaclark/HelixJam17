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

	public float boostStrength = 0.0f;
	[Range(0.0f, 1.0f)]
	public float boostDecel = 0.0f;

	public float jumpStrength = 0.0f;
	public int jumpsAllowed = 0;
	private int _jumps = 0;

	public Text debugText = null;
	public Text jumpText = null;

	private List<Vector3> _deltas = new List<Vector3> ();
	private bool _boosting = false;
	private float _realBoost = 0.0f;

	private void Start()
	{
		for (int i = 0; i < waves.Count; ++i)
		{
			waves [i].LineCrossed += OnLineCrossed;
			_deltas.Add (Vector3.zero);
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
			
		if (Input.GetKeyDown (KeyCode.W) && _jumps > 0)
		{
			Boost ();
		}
		else
		{
			SlowBoost ();
		}

		Gravity ();

		VerticalSpeed ();

		Apply ();
	}

	private void Jump()
	{
		_jumps--;
		_jumps = Mathf.Clamp (_jumps, 0, jumpsAllowed);
		jumpText.text = _jumps.ToString ();

		for (int i = 0; i < waves.Count; ++i)
		{
			if (waves [i].transform.position.x > 0)
			{
				Vector3 delta = _deltas [i];
				float aboveZeroX = delta.x;
				if (aboveZeroX < 0)
				{
					aboveZeroX = 0;
				}
				delta.x = aboveZeroX + jumpStrength;
				_deltas [i] = delta;
			}
			else
			{
				Vector3 delta = _deltas [i];
				float belowZeroX = delta.x;
				if (belowZeroX > 0)
				{
					belowZeroX = 0;
				}
				delta.x = belowZeroX - jumpStrength;
				_deltas [i] = delta;
			}
		}
	}

	private void Boost()
	{
		_jumps--;
		_jumps = Mathf.Clamp (_jumps, 0, jumpsAllowed);
		jumpText.text = _jumps.ToString ();

		_realBoost = boostStrength;
		_boosting = true;

		for (int i = 0; i < waves.Count; ++i)
		{
			Vector3 delta = _deltas [i];
			delta.x = 0.0f;
			_deltas [i] = delta;
		}
	}

	private void SlowBoost()
	{
		if (!Input.GetKey (KeyCode.W))
		{
			_boosting = false;
		}

		if (!_boosting)
		{
			_realBoost -= _realBoost * boostDecel;
		}
	}

	private void Gravity()
	{
		float realGravity = 0.0f;
		if (_boosting)
		{
			realGravity = boostGravity;
		}
		else
		{
			realGravity = gravity;
		}

		for (int i = 0; i < waves.Count; ++i)
		{
			Vector3 delta = _deltas [i];
			delta.x += -waves [i].transform.position.x * realGravity;
			_deltas [i] = delta;
		}
	}

	private void VerticalSpeed()
	{
		for (int i = 0; i < waves.Count; ++i)
		{
			Vector3 delta = _deltas [i];
			delta.y = speed + _realBoost;
			_deltas [i] = delta;
		}
	}

	private void Apply()
	{
		for (int i = 0; i < waves.Count; ++i)
		{
			waves [i].ApplyDelta (_deltas [i]);
		}
	}

	private void OnLineCrossed(Wave wave)
	{
		_jumps = jumpsAllowed;
		jumpText.text = _jumps.ToString ();
		_boosting = false;
	}

	private void OnDestroy()
	{
		for (int i = 0; i < waves.Count; ++i)
		{
			waves [i].LineCrossed -= OnLineCrossed;
		}
	}
}
