using UnityEngine;
using System.Collections;

public class RoomObscure : MonoBehaviour {
	private MeshRenderer r;

	void Awake() {
		r = GetComponent<MeshRenderer>();
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player") 
			r.enabled = false;
	}

	//void OnTriggerExit(Collider other) {
	//	if(other.gameObject.tag == "Player") 
	//		r.enabled = true;
	//}
}
