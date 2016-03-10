using UnityEngine;
using System.Collections;

public class Activatable : MonoBehaviour{
	//Activatables are things that can be turned on or off, like switches, torches, and spikes

	public bool activated = false;
	private Animator animator;
	private BoxCollider col;
	public bool disableCollider = true;
	void Awake() {
		col = GetComponent<BoxCollider>();
		animator = GetComponent<Animator>();
	}
	
	public void Activate() {
		//Assumes two animator bools: "Activated" and "Deactivated"
		if(animator!=null) {
			animator.SetBool ("Activated", true);
			animator.SetBool ("Deactivated", false);
		}
		if(col != null && disableCollider) col.enabled = true;
		if(!activated && audio != null) audio.Play();
		activated = true;
	}

	public void Deactivate() {
		//Assumes two animator bools: "Activated" and "Deactivated"
		if(animator!=null) {
			animator.SetBool ("Deactivated", true);
			animator.SetBool ("Activated", false);
		}
		if(col != null && disableCollider) col.enabled = false;
		activated = false;
	}
}
