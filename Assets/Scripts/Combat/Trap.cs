using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {

	public GameObject[] gates;
	public AudioClip trapMusic;
	public Transform[] enemies;
	private CamLooker looker; //Camera

	private Flags flags;
	private MusicManager music;
	private AudioClip originalMusic;
	private Activatable[] gateScripts;
	private int i3 = 0;
	private int numberAlive;
	private bool trapActivated;
	private bool trapCleared = false;
	public Transform player;
	public TextAsset trapPrompt;
	private GameObject canvas;
	GameData data;

	// Use this for initialization
	void Awake() {
		//player = GameObject.FindGameObjectWithTag("PlayerSwapper").transform;
		numberAlive = enemies.Length;
		music = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManager>();
		//gates = GameObject.FindGameObjectsWithTag ("Gate");
		setUpGates();
		looker = GameObject.FindGameObjectWithTag("CamFollow").GetComponent<CamLooker>();
		GameObject c = GameObject.FindGameObjectWithTag ("GameController");
		data = c.GetComponent<GameData>();
		flags = c.GetComponent<Flags>();

		flags.AddTrapFlag();
		trapCleared = flags.CheckTrapFlag();
		canvas = GameObject.Find ("HUD").gameObject;
	}
	
	// Update is called once per frame
	void Update () {

		if(numberAlive > 0) checkEnemyDeath();

		if (numberAlive <= 0 && gateScripts[0].activated == true) {
			deactivateGates();
			music.stopMusic();
			audio.Play();
			music.changeMusic(music.previousMusic, 5f);
			trapCleared = true;
			flags.SetTrapToCleared();
		}

	}

	bool trapPreDisarmed() {
		foreach (Transform e in enemies) {
			if(e!=null) return false;
		}
		return true;
	}

	void OnTriggerEnter(Collider other) {
		if (trapPreDisarmed ()) {
			if(other.tag == "Player" && !trapCleared) {
				Time.timeScale = 0; //Pause
				music.changeMusic(music.previousMusic, 0.1f);
				StartCoroutine(disarmTrap());
				GameObject effect = Resources.Load("Effects/DisarmEffect") as GameObject;
				Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane)); 
				Instantiate(effect, pos, Quaternion.identity);
				trapCleared = true;
				flags.SetTrapToCleared();
			}
		}
		else if(!trapActivated) {
			if(other.tag == "Player" && !trapCleared) {
				player = other.transform;
				Time.timeScale = 0; //Pause
				music.stopMusic();
				activateGates();
				trapActivated = true;
				GameObject effect = Resources.Load("Effects/TrapEffect") as GameObject;
				Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane)); 
				Instantiate(effect, pos, Quaternion.identity);
			}
		}

	}

	void checkEnemyDeath() {
		if (numberAlive >= 0 && enemies [i3] == null) {
			numberAlive--;
			i3++;
		}
	}

	void setUpGates() {
		gateScripts = new Activatable[gates.Length];
		for (int i=0; i < gates.Length; i++) {
			gateScripts[i] = gates[i].GetComponent<Activatable>();
		}

	}

	void activateGates() {
		//foreach (Activatable gate in gateScripts) {
		//	gate.Activate();
		//}
		//Time.timeScale = 1;
		StartCoroutine(startTrap());
	}

	void deactivateGates() {
		foreach (Activatable gate in gateScripts) {
			gate.Deactivate();
		}
	}

	IEnumerator disarmTrap() {
		yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(4f));
		yield return StartCoroutine(DialogueDisplay.DisplaySpeech("DisarmTrap"));
		Time.timeScale = 1;
	}

	IEnumerator startTrap() {
		canvas.SetActive (false);
		yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(0.8f));
		for (int i=0; i < gates.Length; i++) {
			yield return StartCoroutine(looker.lookAtTarget(gates[i].transform, 35f));
			gateScripts[i].Activate();
			yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(0.5f));
		}
		//Finish it!
		yield return StartCoroutine(looker.lookAtTarget(player, 20f));
		yield return StartCoroutine(DisplayDialogue.Speak(trapPrompt));
		music.changeMusic(trapMusic, 0.1f);
		canvas.SetActive (true);
		Time.timeScale = 1;
	}
	
}
