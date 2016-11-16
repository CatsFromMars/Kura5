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
	private bool isSkipped = false;
	public Animator[] pieces;
	private AudioClip music;
	public GameObject ops;

	void Awake() {
		AudioListener.volume = 1;
		Time.timeScale = 1;
		int percent = GenericPattern.getPercent();
		if(percent >= 100 || percent <= 0) isNightTime = true;
		else isNightTime = false;
		if (isNightTime) {
			Camera.main.backgroundColor = nightColor;
			AudioSource a = GetComponent<AudioSource>();
			music = nightMusic;
			dayClouds.gameObject.SetActive(false);
			nightClouds.gameObject.SetActive(true);
		}
		else {
			Camera.main.backgroundColor = dayColor;
			AudioSource a = GetComponent<AudioSource>();
			music = dayMusic;
			nightClouds.gameObject.SetActive(false);
			dayClouds.gameObject.SetActive(true);
		}

		StartCoroutine (idle());
	}

	// Update is called once per frame
	void Update () {

		if(!isSkipped&&Input.GetButtonDown("Inventory")) skip();
		else if (Input.GetButtonDown("Inventory") && Time.timeScale!=0) {
			//changingScene = true;
			start.SetActive(false);
			ops.SetActive(true);

		}
		//if(!audio.isPlaying && !changingScene) {
		//	changingScene = true;
		//	fader.gotoScene("MenuTutorial1");
		//}
	
	}

	void skip() {
		foreach(Animator piece in pieces) {
			piece.SetBool(Animator.StringToHash("Skip"),true);
			setIsSkipped();
		}
		playMusic();
	}

	void setIsSkipped() {
		isSkipped = true;
	}

	void playIntro() {
		audio.Play ();
	}
	void playMusic() {
		audio.Stop ();
		audio.clip = music;
		audio.Play ();
	}

	void enableText() {
		start.active = true;
		copyrightText.active = true;
	}

	IEnumerator idle() {
		yield return new WaitForSeconds(30);
		fader.gotoScene("MenuTutorial1");
	}
}
