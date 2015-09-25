using UnityEngine;
using System.Collections;

public class Activatable : MonoBehaviour{
	//Activatables are things that can be turned on or off, like switches, torches, and spikes

	public bool activated = false;
	private Animator animator;
	private BoxCollider col;
	void Awake() {
		col = GetComponent<BoxCollider>();
		animator = GetComponent<Animator>();
	}
	
	public void Activate() {
		//Assumes two animator bools: "Activated" and "Deactivated"
		animator.SetBool ("Activated", true);
		if(col != null) col.enabled = true;
		activated = true;
		Debug.Log ("ACTIVATED!");
	}

	public void Deactivate() {
		//Assumes two animator bools: "Activated" and "Deactivated"
		animator.SetBool ("Deactivated", true);
		if(col != null) col.enabled = false;
		activated = false;
	}
}
