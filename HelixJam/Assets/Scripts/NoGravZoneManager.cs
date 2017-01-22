using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGravZoneManager : MonoBehaviour {

	public NoGravZone noGravZone = null;
	public int numNoGravZones = 50;

	private Vector3 startPos = new Vector3 (0f, 20.0f, 0f);

	// Use this for initialization
	void Start () {
		RandomNGZLayout (numNoGravZones);
	}

	public void RandomNGZLayout (int _numNoGravZones) 
	{
		float spawnYPos = startPos.y;
		while (_numNoGravZones > 0) 
		{
			float xSpread = Random.Range (6.0f, 10.0f);
			float xScale = Random.Range (5.0f, 8.0f);
			float yScale = Random.Range (5.0f, 10.0f);

			Vector3 leftPos = new Vector3 (-xSpread, spawnYPos, 0f);
			NoGravZone leftZone = Instantiate (noGravZone, leftPos, Quaternion.identity, null);
			leftZone.transform.localScale = new Vector3 (xScale, yScale, 1f);
			leftZone.rectTransform.localScale = new Vector3 (1 / xScale, 1 / yScale, 1);
			leftZone.rectTransform.sizeDelta = new Vector2 (leftZone.rectTransform.sizeDelta.x * xScale, leftZone.rectTransform.sizeDelta.y * yScale);
			Vector3 rightPos = new Vector3 (xSpread, spawnYPos, 0f);
			NoGravZone rightZone = Instantiate (noGravZone, rightPos, Quaternion.identity, null);
			rightZone.transform.localScale = new Vector3 (xScale, yScale, 1f);
			rightZone.rectTransform.localScale = new Vector3 (1 / xScale, 1 / yScale, 1);
			rightZone.rectTransform.sizeDelta = new Vector2 (rightZone.rectTransform.sizeDelta.x * xScale, rightZone.rectTransform.sizeDelta.y * yScale);

			spawnYPos += Random.Range (35.0f, 55.0f);
			_numNoGravZones--;
		}
	}
}
