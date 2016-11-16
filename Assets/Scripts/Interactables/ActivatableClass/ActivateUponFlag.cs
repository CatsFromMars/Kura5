using UnityEngine;
using System.Collections;

public class ActivateUponFlag : Activatable {

	private Flags flags;
	private bool currentlyActivated = false;
	public string otherFlag;
	
	// Use this for initialization
	void Start () {
		flags = GetUtil.getFlags ();
		currentlyActivated = activated;
		if(currentlyActivated) Activate();
	}
	
	// Update is called once per frame
	void Update () {
		if(!currentlyActivated) {
			if(flags.CheckOtherFlag(otherFlag) && !activated) Activate();
			else if(!flags.CheckOtherFlag(otherFlag) && activated) Deactivate();
		}
		else {
			Debug.Log(flags.CheckOtherFlag(otherFlag));
			if(flags.CheckOtherFlag(otherFlag) && activated) Deactivate();
			else if(!flags.CheckOtherFlag(otherFlag) && !activated) Activate();
		}
	}
}
