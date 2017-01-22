using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionZoneManager : MonoBehaviour {

	public DirectionZone dirZone = null;
	public int numDirectionZones = 100;

	private Vector3 startPos = new Vector3 (0f, 25f, 0f);

	// Use this for initialization
	void Start () {
		RandomDirectionZoneLayout (numDirectionZones);
	}

	public void RandomDirectionZoneLayout (int _numDirZones)
	{
		float spawnYPos = startPos.y;
		while (_numDirZones > 0) {
			float xSpread = Random.Range (3.0f, 10.0f);
			float xScale = Random.Range (5.0f, 8.0f);
			float yScale = Random.Range (2.0f, 10.0f);
			float xDir = Random.Range (-1f, 1f);
			float yDir = Random.Range (0, 1.0f);

			Vector3 leftPos = new Vector3 (-xSpread, spawnYPos, 0f);
			DirectionZone leftZone = Instantiate (dirZone, leftPos, Quaternion.identity, null);
			leftZone.transform.localScale = new Vector3 (xScale, yScale, 1f);
			leftZone.direction = new Vector2 (xDir, yDir);

			Vector3 rightPos = new Vector3 (xSpread, spawnYPos, 0f);
			DirectionZone rightZone = Instantiate (dirZone, rightPos, Quaternion.identity, null);
			rightZone.transform.localScale = new Vector3 (xScale, yScale, 1f);
			rightZone.direction = new Vector2(xDir, yDir);

			spawnYPos += Random.Range (35.0f, 55.0f);
			_numDirZones--;
		}
	}

}
