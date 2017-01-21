using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wave : MonoBehaviour 
{
	public Action<Wave> LineCrossed = delegate {};

	private float _boostDeceleration = 0.0f;
	private float _gravity = 0.0f;
	private float _boostGravity = 0.0f;
	private float _boostStrength = 0.0f;
	private float _jumpStrength = 0.0f;
	private float _speed = 0.0f;

	private float _xMove = 0.0f;
	private float _realBoost = 0.0f;
	private float _realJump = 0.0f;

	private bool _positive = true;
	private bool _boosting = false;

	private void Start()
	{
		GetComponent<Renderer> ().enabled = false;

		_positive = transform.position.x >= 0;
	}

	public void Init(float jumpStrength, float boostStrength, float gravity, float boostDecel, float speed, float boostGravity)
	{
		_jumpStrength = jumpStrength;
		_boostStrength = boostStrength;
		_gravity = gravity;
		_boostGravity = boostGravity;
		_boostDeceleration = boostDecel;
		_speed = speed;
	}


	private void Update()
	{
		Vector3 delta = Vector3.zero;

		CheckReleaseBoost ();

		if (_boosting)
		{
			_xMove += -transform.position.x * (_boostGravity);
		}
		else
		{
			_xMove += -transform.position.x * (_gravity);
		}

		CheckLineCross ();

		if (!_boosting)
		{
			_realBoost -= _realBoost * _boostDeceleration;
		}

		delta.x = _xMove;
		delta.y = _speed + _realBoost;

		transform.position += delta * Time.deltaTime;
	}

	public void Jump()
	{
		if (transform.position.x > 0)
		{
			float aboveZeroX = _xMove;
			if (aboveZeroX < 0)
			{
				aboveZeroX = 0;
			}
			_xMove = aboveZeroX + _jumpStrength;
		}
		else
		{
			float belowZeroX = _xMove;
			if (belowZeroX > 0)
			{
				belowZeroX = 0;
			}
			_xMove = belowZeroX - _jumpStrength;
		}	
	}

	public void BoostSpeed()
	{
		_realBoost = _boostStrength;
		_boosting = true;
		_xMove = 0;
	}

	private void CheckReleaseBoost()
	{
		if (!Input.GetKey (KeyCode.W))
		{
			_boosting = false;
		}
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
		_boosting = false;

		LineCrossed (this);
	}
}
