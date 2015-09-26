using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LightLevels : MonoBehaviour {
	//Controller, data storeage, and GUI of the LD Meter
	public int upperBound = 6;
	public int lowerBound = 0;
	public int sunlight = 0;
	public int darkness = 0;

	private GameObject lightSource;
	private GameObject shadowSource;
	//private Vector3 lightVector;
	//private Vector3 shadowVector;

	//private GameObject player;
	private int s1;
	private int d1;

	public Slider solSlider;
	public Slider darkSlider;

	//public SpriteRenderer lightOverlay;

	void Awake() {
		lightSource = GameObject.FindGameObjectWithTag("Sunlight");
		shadowSource = GameObject.FindGameObjectWithTag("Shadow");
		//player = GameObject.FindGameObjectWithTag("Player");
	}

	void OnLevelWasLoaded(int level) {
		lightSource = GameObject.FindGameObjectWithTag("Sunlight");
		shadowSource = GameObject.FindGameObjectWithTag("Shadow");
		//player = GameObject.FindGameObjectWithTag("Player");
	}

	void Update() {
		//if (player == null) {
			//player = GameObject.FindGameObjectWithTag("Player");
		//}
		//else {
			//transform.position = player.transform.position;
		//}
		calc();
		updateGUI ();
	}

	void calc() {
		float distanceSun = Vector3.Distance(lightSource.transform.position, transform.position);
		float distanceDark = Vector3.Distance(shadowSource.transform.position, transform.position);
		s1 = Mathf.FloorToInt(upperBound - (distanceSun / upperBound));
		d1 = Mathf.FloorToInt(upperBound - (distanceDark / upperBound));
		if(s1 > upperBound) s1=upperBound;
		if(d1 > upperBound) d1=upperBound;
		if (s1 < 0) s1 = 0;
		if (d1 < 0) d1 = 0;
		sunlight = (s1 - d1); //+ Mathf.FloorToInt(lightSource.transform.position.y/10);
		darkness = (d1 - s1); //+ Mathf.FloorToInt(shadowSource.transform.position.y/10);
		if (sunlight < 0) sunlight = 0;
		if (darkness < 0) darkness = 0;

		//Debug.Log(s1);
		//Debug.Log (darkness);
		solSlider.value = sunlight;
		darkSlider.value = darkness;

	}

	void updateGUI() {
		Color a = new Color (255, 255, 255, sunlight * 10);
		//lightOverlay.color = a;
	}
}
