using UnityEngine;
using System.Collections;

public class Crystal : MonoBehaviour {
	private bool activated = false;
	public void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" && !activated) {
			activated = true;
			GetComponent<Animator>().SetTrigger(Animator.StringToHash("Trigger"));
		}
	}
}
