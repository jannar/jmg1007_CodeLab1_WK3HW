using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

	// SCRIPTS
	public ColorChangerScript ccs;

	// GAME OBJECTS
	public GameObject square;
	public GameObject square1;
	public GameObject square2;
	public GameObject square3;
	public Text infoText;

	// BUTTON MAPPING
	public KeyCode p1Key;
	public KeyCode p2Key;
	public KeyCode p3Key;
	public KeyCode p4Key;

	// SCREENSHAKE
	// store camera position for screenshake
	Vector3 originalCameraPosition;

	// float for shaking
	float shakeAmt = 0.1f;
	float ranger;		// what's this? A ranger, caught off his guard?

	// CAMERA
	public Camera mainCamera;

	// DO THEY MATCH?
	public bool colorMatch = false;
	public bool colorMatch1 = false;
	public bool colorMatch2 = false;
	public bool colorMatch3 = false;
	public bool allColorMatch = false;
	public bool win = false;

	// FOR HIGH SCORE SAVING
	// where to save
	public string fileName = "temp.txt";
	public string filePath;

	// the scores
	[HideInInspector]
	public List<int> highScoreValues;

	// consts for saving to Player Prefs
	private const string PREF_HIGH_SCORE = "highScorePref";

	// Property for HighScore
	private int highScore = 0;

	public int HighScore{
		get { 
			highScore = PlayerPrefs.GetInt (PREF_HIGH_SCORE);
			return highScore;
		} set { 
			highScore = value;
			PlayerPrefs.SetInt (PREF_HIGH_SCORE, highScore);
		}
	}

	// Property for wins
	private int wins;

	public int Wins{
		get {
			return wins;
		}

		set{
			wins = value;

			// if wins > HighScore, make HighScore = score
			if (wins > HighScore){
				HighScore = wins;
			}
		}
	}


	// SINGLETON
	public static ScoreScript instance;

	void Start(){

		// set up some variables
		ranger = Random.Range(-1.0f, 1.0f);
		mainCamera = Camera.main;

		// find text and objects
		infoText.text = " ";

		ccs = GameObject.Find ("Square").GetComponent<ColorChangerScript> ();

		originalCameraPosition = new Vector3 (mainCamera.transform.position.x,
			mainCamera.transform.position.y, -10);

		filePath = Application.dataPath + "/" + fileName;

		// get the squares
		square = GameObject.Find("Square");
		square1 = GameObject.Find("Square1");
		square2 = GameObject.Find("Square2");
		square3 = GameObject.Find("Square3");

		// key inputs
		p1Key = KeyCode.Q;
		p2Key = KeyCode.P;
		p3Key = KeyCode.Z;
		p4Key = KeyCode.RightShift;

		// streeeeeeeeamreader
		StreamReader sr = new StreamReader(filePath);
		string line = sr.ReadLine ();
		sr.Close ();

		// make it a singleton
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (this);
		} else {
			Destroy (gameObject);
		}
	}

	void Update () {

		// MATCHING
		if (Input.GetKeyDown (p1Key)) {
			Matcher (square);
		}

		if (Input.GetKeyDown (p2Key)) {
			Matcher (square1);
		}

		if (Input.GetKeyDown (p3Key)) {
			Matcher (square2);
		}

		if (Input.GetKeyDown (p4Key)) {
			Matcher (square3);
		}

		// ENDGAME
		//for winning
		if (square.activeInHierarchy) {
			colorMatch = false;
		} else {
			colorMatch = true;
		}

		if (square1.activeInHierarchy) {
			colorMatch1 = false;
		} else {
			colorMatch1 = true;
		}

		if (square2.activeInHierarchy) {
			colorMatch2 = false;
		} else {
			colorMatch2 = true;
		}

		if (square3.activeInHierarchy) {
			colorMatch3 = false;
		} else {
			colorMatch3 = true;
		}

		if (colorMatch && colorMatch1 && colorMatch2 && colorMatch3) {
			allColorMatch = true;
		}

		if (ccs.timeRemaining <= 0) {
			if (allColorMatch) {
				infoText.text = "faster!";
				KillText ();
				wins = wins + 1;
				win = true;
				SceneManager.LoadScene (0);
				ccs.duration = ccs.duration - 1.0f;
			} else if (!allColorMatch) { // for losing
				// and here we record it before we move on
				if (wins > highScore){
					highScoreValues.Add(wins);

					// ** streamwriter **
					StreamWriter sw = new StreamWriter(filePath, true);

					for (int i = 0; i < highScoreValues.Count; i++) {
						sw.WriteLine (highScoreValues [i]);
						Debug.Log (highScoreValues[i] + "recorded");
					}

					sw.Close ();
				}
				infoText.text = "you failed. r to reset.";
				mainCamera.backgroundColor = Color.red;
				if (Input.GetKeyDown (KeyCode.R)) {
					SceneManager.LoadScene (0);
					win = false;
				}
			}
		}
	}

	void Matcher(GameObject go){

		SpriteRenderer sr = go.GetComponent<SpriteRenderer> ();

		if (sr.color == Camera.main.backgroundColor) {
			ccs.changingColor = false;
			go.SetActive (false);
		} else if (sr.color != Camera.main.backgroundColor) {
			InvokeRepeating ("CameraShake", 0, 0.1f);
			Invoke ("StopShaking", 0.3f);

		}
	}

	void CameraShake(){
		float quakeAmt = Random.value * shakeAmt * 2 - shakeAmt;

		Vector3 pp = mainCamera.transform.position;
		pp.x += quakeAmt * ranger;
		mainCamera.transform.position = pp;

		if (pp.x > 7.5f) {
			pp.x = 7.5f;
		}

		if (pp.x < -7.5f) {
			pp.x = -7.5f;
		}
	}

	void StopShaking(){
		CancelInvoke ("CameraShake");
		mainCamera.transform.position = originalCameraPosition;
	}

	void KillText(){
		Invoke ("DelayedFunction", 2.0f);
	}

	void DelayedFunction(){
		infoText.text = " ";
	}
}
