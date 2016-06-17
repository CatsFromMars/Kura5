using UnityEngine;
using System.Collections;
using System;

public class Menu : MonoBehaviour {

	public GameObject start;
	public GameObject copyrightText;
	public Transform dayClouds;
	public Transform nightClouds;
	public AudioClip dayMusic;
	public AudioClip nightMusic;
	public Color dayColor;
	public Color nightColor;
	public SceneTransition fader;
	private bool isNightTime = false;
	private bool changingScene = false;

	void Awake() {
		Time.timeScale = 1;
		int percent = GenericPattern.getPercent();
		if(percent >= 100 || percent <= 0) isNightTime = true;
		else isNightTime = false;
		if (isNightTime) {
			Camera.main.backgroundColor = nightColor;
			AudioSource a = GetComponent<AudioSource>();
			a.clip = nightMusic;
			a.Play();
			dayClouds.gameObject.SetActive(false);
			nightClouds.gameObject.SetActive(true);
		}
		else {
			Camera.main.backgroundColor = dayColor;
			AudioSource a = GetComponent<AudioSource>();
			a.clip = dayMusic;
			a.Play();
			nightClouds.gameObject.SetActive(false);
			dayClouds.gameObject.SetActive(true);
		}
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown("Inventory") && Time.timeScale!=0 && !changingScene) {
			changingScene = true;
			fader.gotoScene("Intro");

		}
		if(!audio.isPlaying && !changingScene) {
			changingScene = true;
			fader.gotoScene("MenuTutorial1");
		}
	
	}

	void playMusic() {
		audio.Play ();
	}

	void enableText() {
		start.active = true;
		copyrightText.active = true;
	}
}
