using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCountdown : MonoBehaviour {

	public CountdownScript cs;

	public ColorChangerScript changer1;
	public ColorChangerScript changer2;
	public ColorChangerScript changer3;
	public ColorChangerScript changer4;
	public ColorChangerScript bgchanger;


	public void SetCountDownNow(){

		changer1 = GameObject.Find ("Square1").GetComponent<ColorChangerScript> ();
		changer2 = GameObject.Find ("Square2").GetComponent<ColorChangerScript> ();
		changer3 = GameObject.Find ("Square3").GetComponent<ColorChangerScript> ();
		changer4 = GameObject.Find ("Square").GetComponent<ColorChangerScript> ();
		bgchanger = Camera.main.GetComponent<ColorChangerScript> ();

		cs = GameObject.Find ("Game Manager").GetComponent<CountdownScript> ();
		cs.countDownDone = true;

		if (cs.countDownDone) {
			Destroy (gameObject);
		}
	}

	void OnDestroy(){
		// change the colors
		changer1.startColors();
		changer2.startColors ();
		changer3.startColors ();
		changer4.startColors ();
		bgchanger.startColors ();
	}
}
