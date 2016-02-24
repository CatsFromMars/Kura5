using UnityEngine;
using System.Collections;

public class CheatCodes : MonoBehaviour {

	//Cheat input for the same of demoing and testing

	public WeatherSync w;
	
	// Update is called once per frame
	void Update () {

		//Cheat: Make favorable weather conditions for Annie
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			w.isNightTime = false;
			w.lightMax = 6;
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			w.finalTemp = -1f;
			w.cloudinessPercentage = 0;
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)) {
			w.finalTemp = -2f;
			w.cloudinessPercentage = 60;
			w.conditionID = 610;
			w.conditionName = "Snow";
		}
	}
}
