using UnityEngine;
using System.Collections;

public class MakeNoiseAnimationEvent : MonoBehaviour {

	public void makeSound(AudioClip clip) {
		//ANIMATION EVENTS FOR ALL THINGS THAT NEED SOUND
		audio.clip = clip;
		audio.Play();
	}
}
