using UnityEngine;
using System.Collections;

public class Otenko : MonoBehaviour {
	public Talk talk;
	Animator a;
	public ParticleSystem p;

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
}
