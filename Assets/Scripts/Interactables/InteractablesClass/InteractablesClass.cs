using UnityEngine;
using System.Collections;

public class InteractablesClass : MonoBehaviour {
	
	GameObject gameData;
	GameData data;

	// Use this for initialization
	void Awake() {
		gameData = GameObject.FindGameObjectWithTag("GameController");
		data = gameData.GetComponent<GameData>();
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "Player") {
			data.nearInteractable = true;

			if(Input.GetButtonDown("Charge")) {
				Interact();
			}
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			data.nearInteractable = false;
		}
	}

	virtual public void Interact() {
		//To be overritted by the child class
	}
}
