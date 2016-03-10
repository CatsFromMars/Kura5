using UnityEngine;
using System.Collections;

public class InBetween : MonoBehaviour {
	public SceneTransition t;

	// Use this for initialization
	void OnDialogueEnd() {
		StartCoroutine(change ());
	}

	IEnumerator change() {
		yield return StartCoroutine(fadeOutMusic());
		t.gotoScene("Chapter2");
	}

	IEnumerator fadeOutMusic() {
		while(AudioListener.volume > 0) {
			AudioListener.volume -= 2*Time.deltaTime;
			yield return null;
		}
	}
}
