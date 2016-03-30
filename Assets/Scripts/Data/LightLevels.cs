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
	public SafeInt upperBound = new SafeInt(6);
	public SafeInt lowerBound = new SafeInt(0);
	public SafeInt sunlight = new SafeInt(0);
	public SafeInt darkness = new SafeInt(0);

	private GameObject lightSource;
	private GameObject shadowSource;
	//private Vector3 lightVector;
	//private Vector3 shadowVector;

	public WeatherSync w;
	//public SpriteRenderer lightOverlay;

	void Awake() {
		//Get LightMax From WeatherSync
		upperBound = w.lightMax;

	}

	void OnLevelWasLoaded(int level) {
		upperBound = w.lightMax;
		sunlight = new SafeInt(0);
		darkness = new SafeInt(0);
	}

	void Update() {
		//if(lightSource!=null && shadowSource!=null) calc();
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "Sunlight") {
			darkness = new SafeInt(0);
			//if(w.isNightTime == false) sunlight = w.lightMax;
			sunlight = w.lightMax;
		}

		else if (other.tag == "Shadow") {
			sunlight = new SafeInt(0);
			darkness = w.lightMax;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Sunlight") {
			sunlight = new SafeInt(0);
		}
		
		else if (other.tag == "Shadow") {
			darkness = new SafeInt(0);
		}
	}
}
