using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {

	bool initialized = false;
	public GameObject[] gates;
	public bool isMiniboss=false;
	public AudioClip minibossMusic;
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
	public TextAsset disarmPrompt;
	private GameObject canvas;
	GameData data;
	GameOverHandler gameOverHandler;
	private bool trapLost = false;
	private SceneTransition transition;

	//Stealth variables
	private PatrolEnemy[] ais;
	public enum trapType {COMBAT, STEALTH, CUTSCENE}
	public trapType type = trapType.COMBAT;
	public enum stealthType {TREASURE, SWITCH}
	public stealthType stealthMethod = stealthType.TREASURE;
	private TreasureChest[] chests;

	// Use this for initialization
	void Awake() {
		Time.timeScale = 0;
		if(!initialized) initialize();
	}

	public bool isTrapClear() {
		return trapCleared;
	}

	public bool isTrapActivated() {
		return trapActivated;
	}

	void initialize() {
		//player = GameObject.FindGameObjectWithTag("PlayerSwapper").transform;
		numberAlive = enemies.Length;
		music = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManager>();
		//gates = GameObject.FindGameObjectsWithTag ("Gate");
		setUpGates();
		looker = GameObject.FindGameObjectWithTag("CamFollow").GetComponent<CamLooker>();
		GameObject c = GameObject.FindGameObjectWithTag ("GameController");
		data = c.GetComponent<GameData>();
		gameOverHandler = c.GetComponent<GameOverHandler>();
		flags = c.GetComponent<Flags>();
		transition = GameObject.FindGameObjectWithTag("Fader").GetComponent<SceneTransition>();
		flags.AddTrapFlag();
		trapCleared = flags.CheckTrapFlag();
		canvas = GameObject.Find ("HUD").gameObject;
		if(type == trapType.STEALTH) stealthPrep();
		initialized = true;
	}
	
	// Update is called once per frame
	void Update () {

		if(!trapCleared) {
			if(type == trapType.STEALTH) {
				if(checkForStealthGameOver() && !trapLost) {
					//Do coroutine
					//Access data, call game over from here.
					Debug.Log("DEAD");
					trapLost = true;
					if(stealthMethod == stealthType.TREASURE) StartCoroutine(blowUpTreasureChests());
				}
				if(checkForStealthVictory()) {
					BlowUpEnemies();
					deactivateGates();
					music.stopMusic();
					audio.Play();
					music.changeMusic(music.previousMusic, 5f);
					trapCleared = true;
					flags.SetTrapToCleared();
				}
			}

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
	}

	void stealthPrep() {
		//Get treasure chests if applicable
		if (stealthMethod == stealthType.TREASURE) {
			GameObject[] chestObjects = GameObject.FindGameObjectsWithTag("Treasure");
			chests = new TreasureChest[chestObjects.Length];
			int j = 0;
			foreach(GameObject c in chestObjects) {
				chests[j] = c.GetComponent<TreasureChest>();
				j++;
			}
		}

		//Get AIs from each enemy
		ais = new PatrolEnemy[enemies.Length];
		int i = 0;
		foreach (Transform e in enemies) {
			PatrolEnemy p = null;
			if(e!=null) p = e.GetComponent<PatrolEnemy>();
			if(p!=null) ais[i] = p;
			i++;
		}
	}

	bool checkForStealthGameOver() {
		foreach (PatrolEnemy ai in ais) {
			if(ai!=null) {
				if(ai.hasPlayerBeenSpotted()) return true;
			}
		}
		return false;
	}

	bool checkForStealthVictory() {
		if (stealthMethod == stealthType.TREASURE) {
			foreach(TreasureChest c in chests) {
				if(c.chestOpened() == false) return false;
			}
			return true;
		}
		return false;
	}

	IEnumerator blowUpTreasureChests() {
		yield return new WaitForSeconds (1f);
		PauseEnemies ();
		PausePlayer ();
		foreach(TreasureChest c in chests) {
			yield return StartCoroutine(looker.lookAtTarget(c.transform, 35f));
			Instantiate((Resources.Load("Effects/ExplosionEffect")), c.transform.position, Quaternion.Euler(90,0,0));
			c.gameObject.SetActive(false);
		}
		yield return new WaitForSeconds (1f);
		gameOverHandler.setGameOver();

	}

	void BlowUpEnemies() {
		foreach (PatrolEnemy ai in ais) {
			if(ai!=null) {
				ai.Kill();
			}
		}
	}

	void PauseEnemies() {
		foreach (PatrolEnemy ai in ais) {
			if(ai!=null) {
				ai.getAnimator().speed = 0;
			}
		}
	}

	void PausePlayer() {
		player.GetComponent<PlayerContainer> ().playerInControl = false;
	}

	bool trapPreDisarmed() {
		foreach (Transform e in enemies) {
			if(e!=null) return false;
		}
		return true;
	}

	void OnTriggerEnter(Collider other) {
		if(!initialized) initialize();
		if(!trapActivated) {
			if(other.tag == "Player" && !trapCleared) {
				springTrap(other.transform);
				//GameObject effect = Resources.Load("Effects/TrapEffect") as GameObject;
				//Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane+1)); 
				//Instantiate(effect, pos, Quaternion.identity);
			}
		}

	}

	public void springTrap(Transform p) {
		player = p;
		StartCoroutine(startTrap());
		trapActivated = true;
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

	void deactivateGates() {
		foreach (Activatable gate in gateScripts) {
			gate.Deactivate();
		}
	}

	IEnumerator disarmTrap() {
		yield return initialized == true;
		yield return transition.loadingScene == false;
		yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(4f));
		yield return StartCoroutine(DisplayDialogue.Speak(disarmPrompt));
		music.changeMusic(music.previousMusic, 0.1f);
		Time.timeScale = 1;
	}

	IEnumerator startTrap() {

		music.stopMusic();
		yield return initialized == true;
		yield return transition.loadingScene == false;
		yield return new WaitForSeconds (0.1f);
		Time.timeScale = 0; //Pause
		GameObject effect = Resources.Load("Effects/TrapEffect") as GameObject;
		Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane+1)); 
		Instantiate(effect, pos, Quaternion.identity);
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
		if(isMiniboss)music.changeMusic(minibossMusic, 0.1f); 
		else music.changeMusic(trapMusic, 0.1f);
		canvas.SetActive (true);
		Time.timeScale = 1;
	}
	
}
