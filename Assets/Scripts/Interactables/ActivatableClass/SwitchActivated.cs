using UnityEngine;
using System.Collections;

public class SwitchActivated : Activatable {

	public Transform assignedSwitch;
	private Activatable switchScript;

	// Use this for initialization
	void Start () {
		switchScript = assignedSwitch.GetComponent<Activatable>();
	}
	
	// Update is called once per frame
	void Update () {
		if(switchScript.activated && !activated) Activate();
		else if(!switchScript.activated && activated) Deactivate();
	}
}
