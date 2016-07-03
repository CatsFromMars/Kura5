using UnityEngine;
using System.Collections;

public class CheatCodes : MonoBehaviour {

	//Cheat input for the same of demoing and testing

	public WeatherSync w;
	public GameData d;
	
	// Update is called once per frame
	void Update () {

		//Cheat: Make favorable weather conditions for Annie
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			w.conditionID = new SafeInt(601);
			w.lightMax = new SafeInt(2);
			w.finalTemp = new SafeInt(0);
			w.conditionName = "snow";
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			w.isNightTime = false;
			w.conditionID = new SafeInt(800);
			w.lightMax = new SafeInt(7);
			w.conditionName = "clear";
			w.finalTemp = new SafeInt(30);
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)) {
			w.conditionID = new SafeInt(530);
			w.lightMax = new SafeInt(3);
			w.cloudinessPercentage = new SafeInt(51);
			w.conditionName = "rain";
			w.finalTemp = new SafeInt(21);
		}
		if(Input.GetKeyDown(KeyCode.Alpha4)) {
			w.isNightTime = true;
			w.conditionID = new SafeInt(213);
			w.lightMax = new SafeInt(0);
			w.cloudinessPercentage = new SafeInt(99);
			w.finalTemp = new SafeInt(22);
			w.conditionName = "rainstorm";
		}
		if(Input.GetKeyDown(KeyCode.Alpha5)) {
			w.conditionID = new SafeInt(702);
			w.lightMax = new SafeInt(4);
			w.cloudinessPercentage = new SafeInt(75);
			w.conditionName = "Mist";
		}
	}
}
