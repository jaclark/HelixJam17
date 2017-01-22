using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionZone : MonoBehaviour 
{
	public Renderer renderer = null;
	public Vector3 direction = Vector3.zero;
	public Transform particleTransform = null;
	public ParticleSystem particles = null;
	public ParticleSystemRenderer particleRenderer = null;
	private bool _reverse = false;

	private void Start()
	{
		//renderer.enabled = false;
	}

	/*
	private void Update()
	{
		if(Vector3.Distance(transform.position, Camera.main.transform.position) > 10)
		{
			particleRenderer.enabled = false;
		}
	}
	*/

	private void OnGUI()
	{
		Debug.DrawLine (transform.position, transform.position + direction * 10);
	}

	public void SetPosition(Vector3 position)
	{
		transform.position = position;
		//renderer.material.SetVector ("_Position", position);
	}

	public void SetDirection(Vector3 dir)
	{
		direction = dir;
		direction.Normalize ();
		particleTransform.SetParent (null);
		if (!_reverse)
		{
			Quaternion quat = Quaternion.FromToRotation (Vector3.up, new Vector3(-direction.x, direction.y, 0));
			particleTransform.rotation *= quat;
			particleTransform.position = transform.position - new Vector3 (-direction.x, direction.y, 0) * 10;
			ParticleSystem.MainModule main = particles.main;
			main.startRotation = -particleTransform.rotation.eulerAngles.z* Mathf.Deg2Rad;
		}
		else
		{
			Quaternion quat = Quaternion.FromToRotation (Vector3.up, new Vector3(direction.x, direction.y, 0));
			particleTransform.rotation *= quat;
			particleTransform.position = transform.position - new Vector3 (direction.x, direction.y, 0) * 10;
			ParticleSystem.MainModule main = particles.main;
			main.startRotation = -particleTransform.rotation.eulerAngles.z * Mathf.Deg2Rad;
		}

		particles.Clear ();
		particles.Simulate (particles.duration);
		particles.Play ();
		//renderer.material.SetVector ("_Direction", direction);
	}

	public void SetReverse(bool reverse)
	{
		_reverse = reverse;
		int reverseInt = reverse ? 1 : 0;
		//renderer.material.SetInt("_Reverse", reverseInt);
	}

	public void SetScale(Vector3 scale)
	{
		transform.localScale = scale;

		particleRenderer.material.SetFloat ("_LeftEdge", transform.position.x - transform.localScale.x / 2f);
		particleRenderer.material.SetFloat ("_RightEdge", transform.position.x + transform.localScale.x / 2f);
		particleRenderer.material.SetFloat ("_TopEdge", transform.position.y + transform.localScale.y / 2f);
		particleRenderer.material.SetFloat ("_BottomEdge", transform.position.y - transform.localScale.y / 2f);
	}
}
