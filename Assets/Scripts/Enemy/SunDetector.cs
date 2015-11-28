using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SunDetector : MonoBehaviour {
	//used for enemies
	private LightLevels lightLevels;
	public int sunlight;

	void Awake() {
		lightLevels = GameObject.FindGameObjectWithTag("LightLevels").GetComponent<LightLevels>();
	}
	
	void Update() {
		sunlight = lightLevels.calcSunForTarget (this.transform);
	}

}
