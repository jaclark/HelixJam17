using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCamera : MonoBehaviour 
{
	public Transform aWave = null;
	public Transform bWave = null;

	private void LateUpdate()
	{
		float yAvg = (aWave.position.y + bWave.position.y) / 2f;

		transform.position = new Vector3 (0, yAvg, -10);
	}
}
