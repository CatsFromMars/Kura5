using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	Animator animator;

	void Awake() {
		animator = GetComponent<Animator>();
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player" || other.tag == "EnemyWeapon") 
			animator.SetBool(Animator.StringToHash("PlayerInRange"), true);
	}

	void OnTriggerExit(Collider other) {
		if(other.tag == "Player" || other.tag == "EnemyWeapon") 
			animator.SetBool(Animator.StringToHash("PlayerInRange"), false);
	}
}
