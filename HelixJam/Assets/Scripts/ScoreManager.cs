using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
	private static ScoreManager _instance;

	public static ScoreManager Instance { get { return _instance; } }

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
	}

	private int _blueScore = 0;
	private int _orangeScore = 0;
	public Text OrangeScoreText;
	public Text BlueScoreText;

	public void IncreaseBlueScore() {
		_blueScore++;
		BlueScoreText.text = _blueScore.ToString();
	}

	public void IncreaseOrangeScore() {
		_orangeScore++;
		OrangeScoreText.text = _orangeScore.ToString();
	}

}
