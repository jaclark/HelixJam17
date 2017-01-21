using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledWave : MonoBehaviour 
{
	private const float NEARING_DISTANCE = 0.5f;
	private const float LEAVING_DISTANCE = 0.0f;

	public float frequency = 0.0f;
	public float speed = 0.0f;
	public float minAmplitude = 0.0f;
	public float baseAmplitude = 0.0f;
	public float maxAmplitude = 0.0f;

	private float _amplitude = 0.0f;

	private bool _checking = false;
	private bool _positive = false;

	private bool _pressedOnNearing = false;
	private bool _pressedOnLeaving = false;

	private void Start()
	{
		GetComponent<Renderer> ().enabled = false;
		_amplitude = baseAmplitude;
	}

	private void Update()
	{
		UpdateAmplitudes ();
		float x = Mathf.Sin (Time.time * frequency) * _amplitude;
		float y = transform.position.y + speed * Time.deltaTime;
		transform.position = new Vector3 (x, y, 0);
	}

	private void UpdateAmplitudes()
	{
		if(!_checking)
		{
			if (_positive)
			{
				if (transform.position.x < NEARING_DISTANCE)
				{
					OnNearingLine ();
				}
			}
			else
			{
				if (transform.position.x > -NEARING_DISTANCE)
				{
					OnNearingLine ();
				}
			}
		}
		else
		{
			if (_positive)
			{
				if (transform.position.x < 0.0f)
				{
					OnLeavingLine ();
				}
			}
			else
			{
				if (transform.position.x > 0.0f)
				{
					OnLeavingLine ();
				}
			}
		}
	}

	private void OnNearingLine()
	{
		_pressedOnNearing = Input.GetKey (KeyCode.Space);

		_checking = true;
	}

	private void OnLeavingLine()
	{
		_pressedOnLeaving = Input.GetKey (KeyCode.Space);

		DetermineNewAmplitude ();

		_checking = false;
		_positive = !_positive;
	}

	private void DetermineNewAmplitude()
	{
		if (!_pressedOnNearing)
		{
			_amplitude = baseAmplitude;
		}
		if (_pressedOnNearing && _pressedOnLeaving)
		{
			_amplitude = minAmplitude;
		}
		else if(_pressedOnNearing && !_pressedOnLeaving)
		{
			_amplitude = maxAmplitude;
		}
	}
}
