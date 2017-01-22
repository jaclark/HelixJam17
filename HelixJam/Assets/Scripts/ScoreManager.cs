using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
	private int _extraLifeCountBlue = 3;
	private int _extraLifeCountOrange = 3;
	public Text OrangeScoreText;
	public Text BlueScoreText;
	public GameObject FailureScreen;
	public Text FailureText;
	public Wave AWave;
	public Wave BWave;

	public int Lives = 3;

	public void IncreaseBlueScore() {
		_blueScore++;
		if(_extraLifeCountBlue > 0) _extraLifeCountBlue--;
		HandleExtraLife ();
		BlueScoreText.text = _blueScore.ToString();
	}

	public void IncreaseOrangeScore() {
		_orangeScore++;
		if(_extraLifeCountOrange > 0) _extraLifeCountOrange--;
		HandleExtraLife ();
		OrangeScoreText.text = _orangeScore.ToString();
	}

	public void SubtractLife() {
		Lives--;
		if (Lives <= 0)
			StartCoroutine (BeginRestart());
	}

	public void AddLife() {
		Lives++;
	}

	public void HandleExtraLife() {
		if (_extraLifeCountOrange == 0 && _extraLifeCountBlue == 0) {
			_extraLifeCountBlue = 3;
			_extraLifeCountOrange = 3;
			AddLife ();
			AWave.GrowTrail ();
			BWave.GrowTrail ();
		}
	}

	private IEnumerator BeginRestart() {
		WaveMaster.Instance.speed = 0.0f;
		FailureScreen.SetActive (true);
		FailureText.text = "Restarting in 3";
		yield return new WaitForSeconds (1f);
		FailureText.text = "Restarting in 2";
		yield return new WaitForSeconds (1f);
		FailureText.text = "Restarting in 1";
		yield return new WaitForSeconds (1f);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}
}
