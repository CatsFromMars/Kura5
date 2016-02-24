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
		StartCoroutine(load());
	}

	IEnumerator load() {
		yield return w.conditionName != "" && w.status != "";
		yield return new WaitForSeconds(0.5f);
		if(w.isNightTime) src.clip = nightMusic;
		else src.clip = dayMusic;
		src.Play ();
		previousMusic = src.clip;
	}

	void OnLevelWasLoaded(int level) {

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
		//previousMusic = src.clip;
		src.clip = music;
		if(!src.isPlaying) src.PlayDelayed(delay);
	}
}
