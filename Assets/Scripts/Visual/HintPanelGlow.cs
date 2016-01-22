using UnityEngine;
using System.Collections;

public class HintPanelGlow : MonoBehaviour {

	private bool playerInRange = false;
	private int h;
	Animator a;

	void Awake() {
		h = Animator.StringToHash ("PlayerInRange");
		a = GetComponent<Animator>();
	}

	void OnTriggerStay(Collider other) {
		if(other.tag == "Player") {
			playerInRange = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.tag == "Player") {
			playerInRange = false;
		}
	}

	void Update() {
		a.SetBool(h, playerInRange);
	}
}
