using UnityEngine;
using System.Collections;

public class MusicChanger : MonoBehaviour {
	public AudioClip dayMusic;
	public AudioClip nightMusic;
	// Use this for initialization
	void Awake () {
		MusicManager music = GameObject.FindGameObjectWithTag ("Music").GetComponent<MusicManager>();
		WeatherSync w = GameObject.FindGameObjectWithTag ("Weather").GetComponent<WeatherSync>();
		if(w.isNightTime) music.nightMusic=nightMusic;
		else music.dayMusic = dayMusic;
		music.load ();
	}
}
