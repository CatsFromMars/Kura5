using UnityEngine;
using System.Collections;

public class LensGUI : MonoBehaviour {
	public Transform darkLens;
	public Transform solLens;
	public Transform fireLens;
	public Transform frostLens;
	public Transform earthLens;
	public Transform cloudLens;
	public Transform lunaLens;
	public Transform astroLens;
	public string curDisplay = ""; //current display lens

	int index = 0;

	// Update is called once per frame
	public void swapTo (string lens) {
		int angle = 0;
		if (lens == "Dark") angle = 0;
		else if (lens == "Fire") angle = 45;
		else if (lens == "Frost") angle = 90;
		else if (lens == "Earth") angle = 135;
		else if (lens == "Cloud") angle = 180;
		else if (lens == "Luna") angle = 225;
		else if (lens == "Astro") angle = 270;
		else if (lens == "Sol" || lens == "Null") angle = 315;

		StopAllCoroutines ();
		StartCoroutine (spin (angle));
		curDisplay = lens;
	}

	IEnumerator spin(int angle) {
		Quaternion newAngle = Quaternion.Euler(0,angle*-1,180);
		while(transform.rotation != newAngle) {
			transform.rotation = Quaternion.Lerp(transform.rotation, newAngle, Time.unscaledDeltaTime * 5);
			yield return null;
		}
	}
}
