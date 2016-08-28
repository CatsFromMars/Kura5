using UnityEngine;
using System.Collections;

public class Prophecy : MonoBehaviour {
	public TextAsset prophecy;
	public Animator fader;

	void Start() {
		StartCoroutine(introSequence());
	}

	IEnumerator introSequence() {
		yield return StartCoroutine(DisplayDialogue.Speak(prophecy,false,false));
		fader.SetBool(Animator.StringToHash("fadebool"),true);
		float t = 0;
		float f = AudioListener.volume;
		while(t < 2.1f) {
			AudioListener.volume = Mathf.Lerp(f, 0.0f, t);
			yield return null;
			t+=Time.deltaTime;
		}
		audio.Stop ();
		AudioListener.volume = Mathf.Lerp(0.0f, 1.0f, 1);
		Application.LoadLevel("LoadingScene");
	}

}
