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
			w.conditionID = new SafeInt(600);
			w.lightMax = new SafeInt(0);
			w.conditionName = "Light Snow";
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			w.isNightTime = false;
			w.lightMax = new SafeInt(7);
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)) {
			w.conditionID = new SafeInt(530);
			w.lightMax = new SafeInt(2);
			w.conditionName = "Rain";
		}
	}
}
