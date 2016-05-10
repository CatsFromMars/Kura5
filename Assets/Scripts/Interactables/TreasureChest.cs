using UnityEngine;
using System.Collections;

public class TreasureChest : MonoBehaviour {

	Animator animator;
	GameObject gameData;
	GameData data;
	HashIDs hash;
	private MusicManager music;
	Inventory inventory;
	public AudioSource jingle;
	public bool playsJingle;
	public bool stopsMusic = true;

	//Key Item stuff
	public enum itemKind {CONSUMABLE, KEY, LENS, WEAPON};
	public itemKind type;
	public bool pullsUpPrompt = false;
	public int itemID;
	public TextAsset txt;

	//Non key item stuff
	public Transform loot; //ITEM THAT CHEST IS SUPPOSED TO CONTAIN!
	bool itemSpawned = false;

	// Use this for initialization
	void Awake() {
		gameData = GameObject.FindGameObjectWithTag("GameController");
		animator = GetComponent<Animator>();
		data = gameData.GetComponent<GameData>();
		hash = gameData.GetComponent<HashIDs>();
		inventory = gameData.GetComponent<Inventory>();
		music = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManager>();
	}
	
	void OnTriggerStay(Collider other) {
		if (other.tag == "Player") {
			if(Input.GetButtonDown("Charge") && !other.gameObject.GetComponent<PlayerContainer>().performingAction) {
				animator.SetBool (hash.unlockedBool, true);
				if(!itemSpawned) data.nearInteractable = true;
				//audio.Play();

				if(!pullsUpPrompt) spawnLoot();
				else displayPrompt();
			}
		}
		
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Player" && !itemSpawned) {
			animator.SetBool (hash.unlockedBool, false);
			//audio.Play();
		}
		data.nearInteractable = false;
	}

	void displayPrompt() {
		if(!itemSpawned) {
			//Play Jingle
			if(playsJingle) {
				//stops music for 4 seconds, long enough for jingle to play
				if(stopsMusic) music.stopMusic();
				jingle.Play();
				if(stopsMusic) music.changeMusic(music.previousMusic, 4f);
			}
			//Display text
			if(Time.timeScale != 0) StartCoroutine (SpeakCoroutine());
			//Get Loot
			if (type == itemKind.LENS) {
				inventory.AddLens (itemID);
				itemSpawned = true;
			}
			else if (type == itemKind.KEY) {
				inventory.AddKeyItem (itemID);
				itemSpawned = true;
			}
		}
	}

	IEnumerator SpeakCoroutine() {
		yield return StartCoroutine(DisplayDialogue.Speak(txt));
	}

	void spawnLoot() {
		//ANIMATION EVENT
		if(!itemSpawned) {
			Instantiate (loot, transform.position, loot.transform.rotation);
			itemSpawned = true;
		}
	}

	void DestroySelf() {
		Destroy (this.gameObject);
	}

	public bool chestOpened() {
		return itemSpawned;
	}

}
