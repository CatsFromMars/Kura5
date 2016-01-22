using UnityEngine;
using System.Collections;

public class SwitchActivated : Activatable {

	public Transform assignedSwitch;
	private Activatable switchScript;
	private bool currentlyActivated = false;

	// Use this for initialization
	void Start () {
		switchScript = assignedSwitch.GetComponent<Activatable>();
		currentlyActivated = activated;
		if(currentlyActivated) Activate();
	}
	
	// Update is called once per frame
	void Update () {
		if(!currentlyActivated) {
			if(switchScript.activated && !activated) Activate();
			else if(!switchScript.activated && activated) Deactivate();
		}
		else {
			if(switchScript.activated && activated) Deactivate();
			else if(!switchScript.activated && !activated) Activate();
		}
	}
}
