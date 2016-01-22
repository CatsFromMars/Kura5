using UnityEngine;
using System.Collections;

public class Otenko : MonoBehaviour {
	public Talk talk;
	Animator a;
	public ParticleSystem p;
	public AudioClip[] otenkoVoices;
	public AudioSource voice;

	void Awake() {
		a = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		a.SetBool (Animator.StringToHash ("Appeared"), talk.otenkoAppear);
	}

	public void playParticles() {
		p.Play();
	}

	public void randomOtenko() {
		AudioClip o = otenkoVoices[Random.Range(0, otenkoVoices.Length)];
		playVoiceClip(o);
	}

	public void playVoiceClip(AudioClip clip) {
		//ANIMATION EVENTS FOR VOICE ACTING
		voice.clip = clip;
		voice.Play();
	}
}
