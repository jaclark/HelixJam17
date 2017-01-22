using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCamera : MonoBehaviour 
{
	private List<Wave> _waves = null;
	private int _waveCount = 0;

	public JumpUI jumpUI;

	public void Init(List<Wave> waves)
	{
		_waves = waves;
		_waveCount = _waves.Count;
	}

	private void LateUpdate()
	{
		float totalY = 0.0f;

		for (int i = 0; i < _waves.Count; ++i)
		{
			totalY += _waves [i].transform.position.y;
		}
		float yAvg = (totalY) / _waveCount;

		transform.position = new Vector3 (0, yAvg+5f, -10);

		jumpUI.UpdateUI ();
	}
}
