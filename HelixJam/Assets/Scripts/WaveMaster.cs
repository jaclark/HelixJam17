using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveMaster : MonoBehaviour 
{

	private static WaveMaster _instance;

	public static WaveMaster Instance { get { return _instance; } }

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
	}

	public List<Wave> waves = null;
	public BasicCamera basicCamera = null;
	public CenterLine centerLine = null;

	public float speed = 0.0f;
	public float normalSpeed = 0.0f;
	[Range(0.0f, 1.0f)]
	public float gravity = 0.0f;
	[Range(0.0f, 1.0f)]
	public float boostGravity = 0.0f;

	public float boostStrength = 0.0f;
	[Range(0.0f, 1.0f)]
	public float boostDecel = 0.0f;

	public float jumpStrength = 0.0f;
	public int jumpsAllowed = 0;

	public float zoneForce = 0.0f;
	private Vector2 _zoneDirection = Vector2.zero;

	public Text debugText = null;
	public Text jumpText = null;

	private int _jumps = 0;
	private List<Vector3> _deltas = new List<Vector3> ();
	private bool _boosting = false;
	private bool _inNoGrav = false;
	private float _realBoost = 0.0f;

	private void Start()
	{
		for (int i = 0; i < waves.Count; ++i)
		{
			waves [i].LineCrossed += OnLineCrossed;
			waves [i].NoGravZoneEntered += EnableNoGravZone;
			waves [i].NoGravZoneExited += DisableNoGravZone;
			waves [i].EnableZone += EnableZone;
			waves [i].DisableZone += DisableZone;
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

		ZoneForce ();

		Apply ();

		GradualSpeedGain ();
	}

	private void Jump()
	{
		AudioService.Instance.PlayJump ();

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

	private void EnableZone(Vector2 direction)
	{
		_zoneDirection = direction;
	}

	private void DisableZone(Vector2 direction)
	{
		_zoneDirection = Vector2.zero;
	}

	private void ZoneForce()
	{
		for (int i = 0; i < waves.Count; ++i)
		{
			if (waves [i].transform.position.x > 0)
			{
				Vector3 realZoneDirection = new Vector3 (_zoneDirection.x, _zoneDirection.y, 0);
				_deltas [i] += realZoneDirection * zoneForce * Time.deltaTime;
			}
			else
			{
				Vector3 realZoneDirection = new Vector3 (-_zoneDirection.x, _zoneDirection.y, 0);
				_deltas [i] += realZoneDirection * zoneForce * Time.deltaTime;
			}
		}
	}

	private void Boost()
	{
		AudioService.Instance.PlayDash ();

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

	private void EnableNoGravZone (Wave freshWave)
	{
		_inNoGrav = true;
		for (int i = 0; i < waves.Count; ++i)
		{
			Vector3 delta = _deltas [i];
			delta.x = 0.0f;
			_deltas [i] = delta;
		}
	}

	private void DisableNoGravZone (Wave freshWave)
	{
		_inNoGrav = false;
	}

	private void Gravity()
	{
		float realGravity = 0.0f;
		if (_inNoGrav) 
		{
			realGravity = 0.0f;
		}
		else if (_boosting)
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
		if (speed < normalSpeed) {
			speed += Time.deltaTime * 3f;
		} else if (speed > normalSpeed) {
			speed = normalSpeed;
		}

		for (int i = 0; i < waves.Count; ++i)
		{
			Vector3 delta = _deltas [i];
			delta.y = speed + _realBoost;
			ScoreManager.Instance.TotalDistance += Mathf.Floor(delta.y / 8);
			_deltas [i] = delta;
		}
	}

	private void GradualSpeedGain() 
	{
		speed += .02f / 30;
		normalSpeed += .02f / 30;
	}

	private void Apply()
	{
		for (int i = 0; i < waves.Count; ++i)
		{
			waves [i].ApplyDelta (_deltas [i]);
		}

		if (waves.Count >= 2)
		{
			centerLine.SetPositions (waves [0].transform.position.x, waves [1].transform.position.x);
		}
	}

	private void OnLineCrossed(Wave wave)
	{
		AudioService.Instance.PlayMiddle ();
		_jumps = jumpsAllowed;
		jumpText.text = _jumps.ToString ();
		_boosting = false;
	}

	public void HitWall() 
	{
		speed = speed * 0.50f;
		for (int i = 0; i < waves.Count; i++) 
		{
			waves[i].ShrinkTrail();
		}
	}

	private void OnDestroy()
	{
		for (int i = 0; i < waves.Count; ++i)
		{
			waves [i].LineCrossed -= OnLineCrossed;
			waves [i].NoGravZoneEntered -= EnableNoGravZone;
			waves [i].NoGravZoneExited -= DisableNoGravZone;
			waves [i].EnableZone -= EnableZone;
			waves [i].DisableZone -= DisableZone;
		}
	}
}
