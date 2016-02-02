using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SunDetector : MonoBehaviour {
	//used for enemies
	private LightLevels lightLevels;
	public int sunlight;
	public int darkness;

	void Awake() {
		lightLevels = GameObject.FindGameObjectWithTag("LightLevels").GetComponent<LightLevels>();
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Sunlight") {
			darkness = 0;
			//if(w.isNightTime == false) sunlight = w.lightMax;
			if(!lightLevels.w.isNightTime) sunlight = lightLevels.w.lightMax;
		}
		
		else if (other.tag == "Shadow") {
			sunlight = 0;
			darkness = lightLevels.w.lightMax;
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

}
