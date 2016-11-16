using UnityEngine;
using System.Collections;

public class Disclaimer : MonoBehaviour {
	public SceneTransition fader;

	void Awake() {
		StartCoroutine(scene());
	}

	IEnumerator scene() {
		yield return new WaitForSeconds (3);
		fader.gotoScene("MenuScene", false);
	}
}
