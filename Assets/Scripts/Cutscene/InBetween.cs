using UnityEngine;
using System.Collections;
using System.Text;

public class InBetween : MonoBehaviour {
	public TextAsset t;

	void Start() {
		StartCoroutine(inBetween());
		StartCoroutine(fadeInMusic());
	}

	IEnumerator inBetween() {
		yield return StartCoroutine(DisplayDialogue.Speak(t,false,false));
		yield return new WaitForSeconds (0.15f);
		Destroy(GameObject.Find("Global"));
		Application.LoadLevel("MenuScene");
	}

	IEnumerator fadeInMusic() {
		AudioListener.volume = 0;
		while(AudioListener.volume < 1) {
			AudioListener.volume += Time.unscaledDeltaTime;
			yield return null;
		}
		AudioListener.volume = 1;
	}

	IEnumerator fadeOutMusic() {
		while(AudioListener.volume > 0) {
			AudioListener.volume -= 2*Time.deltaTime;
			yield return null;
		}
	}
}
