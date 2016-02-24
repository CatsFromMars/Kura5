using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LightLevels : MonoBehaviour {
	//How big is our skylight?
	public float radius = 5f;
	public float radiusShadow = 15f;
	public float distanceSun;
	public float distanceDark;
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

	public WeatherSync w;
	//public SpriteRenderer lightOverlay;

	void Awake() {
		//Get LightMax From WeatherSync
		upperBound = w.lightMax;

	}

	void OnLevelWasLoaded(int level) {
		upperBound = w.lightMax;
		sunlight = 0;
		darkness = 0;
	}

	void Update() {
		//if(lightSource!=null && shadowSource!=null) calc();
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "Sunlight") {
			darkness = 0;
			//if(w.isNightTime == false) sunlight = w.lightMax;
			sunlight = w.lightMax;
		}

		else if (other.tag == "Shadow") {
			sunlight = 0;
			darkness = w.lightMax;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Sunlight") {
			sunlight = 0;
		}
		
		else if (other.tag == "Shadow") {
			darkness = 0;
		}
	}

	public int calcSunForTarget(Transform target) {
		//utility function for enemies
		float distanceSun = Vector3.Distance(lightSource.transform.position, target.position);
		if (distanceSun <= radius && w.isNightTime == false) {
			return w.lightMax;
		}
		else return 0;
	}
}
