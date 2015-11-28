using UnityEngine;
using System.Collections;

public class Entrance : MonoBehaviour {
	public string nextScene;

	void OnTriggerEnter (Collider other) {
		if(other.tag == "Player") GameObject.FindGameObjectWithTag ("Fader").GetComponent<SceneTransition> ().gotoScene (nextScene);
		//Application.LoadLevel(nextScene);
	}

	void OnTriggerExit(Collider other) {
		if(other.tag == "Player") GameObject.FindGameObjectWithTag ("Fader").GetComponent<SceneTransition>().ren.color = Color.clear;
	}
}
