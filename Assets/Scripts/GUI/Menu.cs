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

	void Awake() {
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

		if (Input.GetKeyDown(KeyCode.Return)) {
			fader.gotoScene("LoadingScene");

		}
	
	}

	void enableText() {
		start.active = true;
		copyrightText.active = true;
	}
}
