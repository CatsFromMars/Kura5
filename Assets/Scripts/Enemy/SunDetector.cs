using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SunDetector : MonoBehaviour {
	//used for enemies
	private LightLevels lightLevels;
	public SafeInt sunlight;
	public SafeInt darkness;

	void Awake() {
		lightLevels = GameObject.FindGameObjectWithTag("LightLevels").GetComponent<LightLevels>();
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Sunlight") {
			darkness = new SafeInt(0);
			//if(w.isNightTime == false) sunlight = w.lightMax;
			if(!lightLevels.w.isNightTime) sunlight = lightLevels.w.lightMax;
		}
		
		else if (other.tag == "Shadow") {
			sunlight = new SafeInt(0);
			darkness = lightLevels.w.lightMax;
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
