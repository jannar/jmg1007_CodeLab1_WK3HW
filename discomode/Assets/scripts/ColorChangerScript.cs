using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangerScript : MonoBehaviour {

	// FOR COLOR CHANGING
	public SpriteRenderer sr;
	public Camera cam;
	// change these out for nicer colors later if time
	public Color[] colors = new Color[] {Color.white, Color.blue, Color.red, Color.cyan, Color.magenta};
	private int currentColor, currentBGColor, length;
	public bool changingColor = false;
	public bool bgChangingColor = false;

	// FOR TIMER
	public float duration;
	public float timeRemaining;
	public bool isCountingDown = false;

	// TICKING PROPERTY
	public float Tick {
		get{ 
			return timeRemaining;
		}

		set{ 
			if (timeRemaining > 0) {
				isCountingDown = true;
			} else {
				if (timeRemaining < 0) {
					timeRemaining = 0;
				}
				isCountingDown = false;
			}
		}
	}
		
	void Start () {

		cam = Camera.main;

		// get their colors + set up the array
		sr = gameObject.GetComponent<SpriteRenderer> ();
		currentColor = 0;
		length = colors.Length;
		sr.color = colors [currentColor];

		// start the timer
		timeRemaining = duration;

	}

	void Update () {

		if (timeRemaining > 0) {
			isCountingDown = true;
		} else {
			isCountingDown = false;
		}
	}

	public IEnumerator colorChanger (GameObject obj, float waitTime){
		while (true) {
			changingColor = true;

			Tick--;

			int randomColorValue = Random.Range (0, colors.Length);

			currentColor = (currentColor + randomColorValue) % length;
			sr.color = colors [currentColor];
			changingColor = false;
			timeRemaining = timeRemaining - waitTime;
			yield return new WaitForSeconds (waitTime);
		}
	}

	public IEnumerator bgColorChanger (Camera cam, float timeWait){
		while (true) {
			bgChangingColor = true;

			int randomColorValue = Random.Range (0, colors.Length);
			currentBGColor = (currentBGColor + randomColorValue) % length;
			Camera.main.backgroundColor = colors[currentBGColor];
			bgChangingColor = false;
			yield return new WaitForSeconds (timeWait);

			
		}
	}

	public void startColors(){

		// change the colors
		StartCoroutine (colorChanger (gameObject, 0.5f));
		StartCoroutine (bgColorChanger (cam, 1.5f));

	}
}
