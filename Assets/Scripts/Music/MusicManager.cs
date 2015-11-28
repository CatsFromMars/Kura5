using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public AudioSource src;
	public AudioClip previousMusic;

	void Awake() {
		src = GetComponent<AudioSource>();
		previousMusic = src.clip;
	}

	public void stopMusic() {
		src.Stop();
	}

	public void startMusic() {
		if(!src.isPlaying) src.Play();
	}

	public void changeMusic(AudioClip music, float delay=0) {
		//Wrapper
		stopMusic();
		previousMusic = src.clip;
		src.clip = music;
		if(!src.isPlaying) src.PlayDelayed(delay);
	}
}
