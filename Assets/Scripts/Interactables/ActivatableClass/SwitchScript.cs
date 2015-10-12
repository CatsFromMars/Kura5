using UnityEngine;
using System.Collections;

public class SwitchScript : Activatable {
	public bool materialChanges = false;
	public SkinnedMeshRenderer currentMat;
	public Material deactiveMat;
	public Material activeMat;
	public enum kindOfSwitch {
		Pressable,
		NotPressable,
	}
	private Transform currentObject;

	public kindOfSwitch kind;

	void Update() {
		if (currentObject == null && activated && kind == kindOfSwitch.NotPressable) {
			Deactivate ();
			if(materialChanges) currentMat.material = deactiveMat;
		}
	}

	// Update is called once per frame
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag != "Enemy"){
			if(kind == kindOfSwitch.NotPressable) {
				Activate ();
				currentObject = other.transform;
				if(materialChanges) currentMat.material = activeMat;
			}
		}
	}

	void OnTriggerStay(Collider other) {
		if (!activated && kind == kindOfSwitch.Pressable) {
			Activate ();
			currentObject = other.transform;
			if(materialChanges) currentMat.material = activeMat;
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.gameObject.tag != "Enemy"){
			if(kind == kindOfSwitch.NotPressable) {
				Deactivate ();
				if(materialChanges) currentMat.material = deactiveMat;
			}
		}
	}
}
