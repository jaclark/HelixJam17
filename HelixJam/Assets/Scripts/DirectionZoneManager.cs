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

			DirectionZone leftZone = Instantiate (dirZone, Vector3.up * -1000, Quaternion.identity, null);
			leftZone.SetReverse (false);
			leftZone.SetPosition (new Vector3 (-xSpread, spawnYPos, 0f));
			leftZone.SetDirection (new Vector3 (xDir, yDir, 0));
			leftZone.SetScale (new Vector3 (xScale, yScale, 1f));

			DirectionZone rightZone = Instantiate (dirZone, Vector3.up * -1000, Quaternion.identity, null);
			rightZone.SetReverse (true);
			rightZone.SetPosition (new Vector3 (xSpread, spawnYPos, 0f));
			rightZone.SetDirection (new Vector2 (xDir, yDir));
			rightZone.SetScale (new Vector3 (xScale, yScale, 1f));

			spawnYPos += Random.Range (35.0f, 55.0f);
			_numDirZones--;
		}
	}

}
