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

	private bool _positive = true;

	private void Start()
	{
		GetComponent<Renderer> ().enabled = false;

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

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "cube") {
			Cube hitCube = (Cube)col.gameObject.GetComponent<Cube> ();
			if (hitCube.cubeType [0] == transform.name [0]) {
				AudioService.Instance.PlayBreak ();
				hitCube.DestroyedParticles.Play ();
				//Destroy (hitCube.gameObject);
				hitCube.gameObject.GetComponent<MeshRenderer>().enabled = false;
			}
		} else if (col.gameObject.tag == "wall") {
			Wall hitWall = (Wall)col.gameObject.GetComponent<Wall> ();
			Renderer wallRend = hitWall.GetComponent<Renderer> ();
			wallRend.material.SetColor ("_Color", Color.gray);
			WaveMaster.Instance.HitWall ();
			AudioService.Instance.PlayHitWall ();
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
}
