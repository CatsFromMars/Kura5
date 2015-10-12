using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	Animator animator;

	void Awake() {
		animator = GetComponent<Animator>();
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player") 
			animator.SetBool(Animator.StringToHash("PlayerInRange"), true);
	}

	void OnTriggerExit(Collider other) {
		if(other.tag == "Player") 
			animator.SetBool(Animator.StringToHash("PlayerInRange"), false);
	}
}
