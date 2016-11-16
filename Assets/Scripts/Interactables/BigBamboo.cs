using UnityEngine;
using System.Collections;

public class BigBamboo : MonoBehaviour {
	private bool inRange = false;
	public GameObject bug;

	// Use this for initialization
	void OnTriggerEnter(Collider other) {

		if(other.tag == "Player"&&!inRange) {
			inRange = true;
			StartCoroutine(bugs());
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.tag == "Player") {
			inRange = false;
		}
	}

	IEnumerator bugs() {
		while(inRange) {
			Instantiate(bug,transform.position, bug.transform.rotation);
			yield return new WaitForSeconds(1);
		}
	}


}
