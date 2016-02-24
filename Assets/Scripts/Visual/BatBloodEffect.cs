using UnityEngine;
using System.Collections;

public class BatBloodEffect : MonoBehaviour {
	//To be put on enemy weapon
	public ParticleSystem blood;
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			blood.Play();
		}
	}
}
