using UnityEngine;
using System.Collections;

public class Entrance : MonoBehaviour {
	public string nextScene;
	private bool canBeTriggered;
	private PlayerContainer player;
	
	void OnTriggerEnter (Collider other) {
		if(other.tag == "Player") startWalking ();
	}

	void OnTriggerExit(Collider other) {
		if(other.tag == "Player") {
			GameObject.FindGameObjectWithTag ("Fader").GetComponent<SceneTransition>().ren.color = Color.clear;
			canBeTriggered = true;
		}
	}

	void startWalking() {
		player = GameObject.FindWithTag ("Player").GetComponent<PlayerContainer> ();
		player.StopAllCoroutines ();
		StartCoroutine (walk());
	}

	IEnumerator walk() {
		Vector3 pos = player.transform.position + player.transform.forward;
		GameObject.FindGameObjectWithTag ("Fader").GetComponent<SceneTransition> ().gotoScene (nextScene);
		//yield return StartCoroutine(player.characterWalkTo(pos, this.transform));
		yield return null;
	}
}
