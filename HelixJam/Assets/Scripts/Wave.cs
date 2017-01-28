using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wave : MonoBehaviour 
{
	public Action<Wave> LineCrossed = delegate {};
	public Action<Wave> NoGravZoneEntered = delegate {};
	public Action<Wave> NoGravZoneExited = delegate {};
	public Action<Vector2> EnableZone = delegate {};
	public Action<Vector2> DisableZone = delegate{};

	public ParticleSystem lifeUpParticles = null;

	private bool _positive = true;
	private TrailRenderer _trailRenderer;
	private float _maxWidth;
	private float _widthChanger;

	private void Start()
	{
		GetComponent<Renderer> ().enabled = false;

		_trailRenderer = GetComponent<TrailRenderer> ();
		_maxWidth = _trailRenderer.startWidth;
		_widthChanger = _maxWidth / ScoreManager.Instance.MaxLives;

		_positive = transform.position.x >= 0;
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

	public void GrowTrail() {
		if(_trailRenderer.startWidth < _maxWidth) 
			_trailRenderer.startWidth += _maxWidth / ScoreManager.Instance.MaxLives;
	}

	public void ShrinkTrail() {
		if (_trailRenderer.startWidth > 0.0f)
			_trailRenderer.startWidth -= _widthChanger;
		else if (_trailRenderer.startWidth < 0.0f) {
			_trailRenderer.startWidth = 0.0f;
		}
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "cube") {
			Cube hitCube = (Cube)col.gameObject.GetComponent<Cube> ();
			if (hitCube.cubeType [0] == transform.name [0]) {
				if (hitCube.cubeType [0] == 'A') 
					ScoreManager.Instance.IncreaseBlueScore();
				else 
					ScoreManager.Instance.IncreaseOrangeScore();
				
				AudioService.Instance.PlayBreak ();
				hitCube.MakeDemParticles ();
				hitCube.renderer.enabled = false;
			}
		} else if (col.gameObject.tag == "wall") {
			Wall hitWall = (Wall)col.gameObject.GetComponent<Wall> ();

			if (hitWall.outline.effectColor != Color.black) {
				hitWall.image.color = new Color (.2f, .2f, .2f, 1f);
				hitWall.outline.effectColor = Color.black;
				WaveMaster.Instance.HitWall ();
				ScoreManager.Instance.SubtractLife ();
				AudioService.Instance.PlayHitWall (); 
			}

		} else if (col.gameObject.tag == "noGravZone") {
			NoGravZoneEntered (this);
			AudioService.Instance.PlayNoGrav ();
		} else if (col.gameObject.tag == "directionZone") {
			DirectionZone dirZone = (DirectionZone)col.gameObject.GetComponent<DirectionZone> ();
			EnableZone (dirZone.direction);
			AudioService.Instance.PlayBumper ();
		}
	}

	void OnTriggerExit (Collider col)
	{
		if (col.gameObject.tag == "noGravZone") {
			NoGravZoneExited (this);
		} else if (col.gameObject.tag == "directionZone") {
			DirectionZone dirZone = (DirectionZone)col.gameObject.GetComponent<DirectionZone> ();
			DisableZone (dirZone.direction);
		}
	}

	public void ChaChing()
	{
		lifeUpParticles.Play ();
	}
}
