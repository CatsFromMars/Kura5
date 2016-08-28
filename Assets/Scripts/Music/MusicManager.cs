using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public AudioSource src;
	public AudioClip previousMusic;
	public AudioClip dayMusic;
	public AudioClip nightMusic;
	public WeatherSync w;

	void Awake() {
		src = GetComponent<AudioSource>();
	}

	void OnLevelWasLoaded(int level) {
		load();
	}

	public void load() {
		AudioClip newClip;
		if(w.isNightTime) newClip = nightMusic;
		else newClip = dayMusic;
		if(newClip != src.clip) {
			src.clip = newClip;
			src.Play();
			previousMusic = src.clip;
		}
	}

	public void stopMusic() {
		src.Stop();
	}

	public void startMusic() {
		if(!src.isPlaying) src.Play();
	}

	public void revertToPrevious() {
		changeMusic(previousMusic);
		startMusic ();
	}

	public void changeMusic(AudioClip music, float delay=0) {
		//Wrapper
		stopMusic();
		//previousMusic = src.clip;
		src.clip = music;
		if(!src.isPlaying) src.PlayDelayed(delay);
	}
}
