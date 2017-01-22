using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpUI : MonoBehaviour 
{
	public LineRenderer linePrefab = null;
	private List<LineRenderer> _lines = null;

	public float totalLineWidth = 0.0f;
	public float lineGap = 0.0f;
	public int lineSegments = 0;

	public float lineAmplitude = 0f;
	public float lineFrequency = 0f;

	private void Start()
	{
		_lines = new List<LineRenderer> ();
		for (int i = 0; i < WaveMaster.Instance.jumpsAllowed; ++i)
		{
			LineRenderer newLine = Instantiate (linePrefab) as LineRenderer;
			_lines.Add (newLine);
			newLine.numPositions = lineSegments;
		}
	}

	public void UpdateUI()
	{
		for (int i = 0; i < _lines.Count; ++i)
		{
			LineRenderer line = _lines [i];

			if (i >= WaveMaster.Instance.jumps)
			{
				for (int j = 0; j < lineSegments; ++j)
				{
					line.SetPosition (j, Vector3.down * 1000);
				}
			}
			else
			{
				float totalWidth = totalLineWidth + lineGap;
				float baseX = (i - ((float)WaveMaster.Instance.jumps / 2f)) * totalWidth + totalWidth / 2f + lineGap;
				float startXPos = baseX - totalWidth/2f;
				float startYPos = transform.position.y;
				for (int j = 0; j < lineSegments; ++j)
				{
					float sinY = Mathf.Sin (startXPos + j * totalLineWidth / lineSegments + Time.time * lineFrequency) * lineAmplitude;
					line.SetPosition (j, new Vector3 (startXPos + j * totalLineWidth / lineSegments, startYPos + sinY, -5));
				}
			}
		}
	}

	public void SetJumps(int jumps)
	{
		
	}
}
