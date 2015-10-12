using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {

	public GameObject[] gates;
	public AudioClip trapMusic;
	public Transform[] enemies;

	private AudioSource srce;
	private AudioClip originalMusic;
	private Activatable[] gateScripts;
	private int i3 = 0;
	private int numberAlive;
	private bool trapActivated;

	private bool trapCleared = false;

	// Use this for initialization
	void Awake() {
		numberAlive = enemies.Length;
		srce = Camera.main.GetComponent<AudioSource>();
		originalMusic = srce.audio.clip;
		//gates = GameObject.FindGameObjectsWithTag ("Gate");
		setUpGates();
	}
	
	// Update is called once per frame
	void Update () {

		if(numberAlive > 0) checkEnemyDeath();

		if (numberAlive <= 0 && gateScripts[0].activated == true) {
			deactivateGates();
			changeMusic();
			trapCleared = true;
		}

	}

	void OnTriggerEnter(Collider other) {
		if(!trapActivated) {
			if(other.tag == "Player" && !trapCleared) {
				changeMusic();
				activateGates();
				trapActivated = true;
			}
		}
	}

	void checkEnemyDeath() {
		if (numberAlive >= 0 && enemies [i3] == null) {
			numberAlive--;
			i3++;
		}
	}

	void changeMusic() {
		//toggles music
		srce.Stop ();
		if (srce.audio.clip != originalMusic) srce.audio.clip = originalMusic;
		else srce.audio.clip = trapMusic;
		srce.Play ();
	}

	void setUpGates() {
		gateScripts = new Activatable[gates.Length];
		for (int i=0; i < gates.Length; i++) {
			gateScripts[i] = gates[i].GetComponent<Activatable>();
		}

	}

	void activateGates() {
		Debug.Log ("Activating Gates");
		foreach (Activatable gate in gateScripts) {
			gate.Activate();
			Debug.Log ("Activated One Gate");
		}
	}

	void deactivateGates() {
		foreach (Activatable gate in gateScripts) {
			gate.Deactivate();
		}
	}
}
