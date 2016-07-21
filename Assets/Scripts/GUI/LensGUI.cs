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

	public Sprite darkText;
	public Sprite solText;
	public Sprite fireText;
	public Sprite frostText;
	public Sprite earthText;
	public Sprite cloudText;
	public Sprite lunaText;
	public Sprite astroText;
	public Sprite noneText;
	public SpriteRenderer curTextSprite;


	private bool isSpinning = false;

	int index = 0;

	void OnEnable() {
			int angle = 0;
			if(curDisplay == "Dark") angle = 0;
			else if(curDisplay == "Fire") angle = 45;
			else if(curDisplay == "Frost") angle = 90;
			else if(curDisplay == "Earth") angle = 135;
			else if(curDisplay == "Cloud") angle = 180;
			else if(curDisplay == "Luna") angle = 225;
			else if(curDisplay == "Astro") angle = 270;
			else if(curDisplay == "Sol" || curDisplay == "Null") angle = 315;
			Quaternion newAngle = Quaternion.Euler(0,angle*-1,180);
			transform.rotation = newAngle;
	}

	// Update is called once per frame
	public void swapTo (string lens) {
		int angle = 0;
		if (lens == "Dark") {
			angle = 0;
			curTextSprite.sprite = darkText;
		}
		else if (lens == "Fire") {
			angle = 45;
			curTextSprite.sprite = fireText;
		}
		else if (lens == "Frost") {
			angle = 90;
			curTextSprite.sprite = frostText;
		}
		else if (lens == "Earth") {
			angle = 135;
			curTextSprite.sprite = earthText;
		}
		else if (lens == "Cloud") {
			angle = 180;
			curTextSprite.sprite = cloudText;
		}
		else if (lens == "Luna") {
			angle = 225;
			curTextSprite.sprite = lunaText;
		}
		else if (lens == "Astro") {
			angle = 270;
			curTextSprite.sprite = astroText;
		}
		else if (lens == "Null") {
			angle = 315;
			curTextSprite.sprite = noneText;
		}
		else if (lens == "Sol") {
			angle = 315;
			curTextSprite.sprite = solText;
		}

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
