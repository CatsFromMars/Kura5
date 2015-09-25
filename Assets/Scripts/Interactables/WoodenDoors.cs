using UnityEngine;
using System.Collections;

//NOTE: THERE IS A BUG WHERE THE DOOR'S LOCKED PROMPT WILL LOOP OVER AND OVER AGAIN
//FIX IT!

public class WoodenDoors : MonoBehaviour {
	public bool doorLocked = true;
	GameObject gameData;
	Inventory inventory;
	Dialogue dialogue;
	HashIDs hash;
	Animator animator;
	public AudioClip doorOpenSound;
	public AudioClip doorClosedSound;
	GameData Data;
	bool inRange = false;
	string[] dialogueSpeech;

	// Use this for initialization
	void Awake () {
		gameData = GameObject.FindGameObjectWithTag("GameController");
		animator = GetComponent<Animator>();
		Data = gameData.GetComponent<GameData>();
		hash = gameData.GetComponent<HashIDs>();
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" || other.tag == "EnemyBody") {
			inRange = true;
			animator.SetBool(hash.inRangeBool, true);
		}
		
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			inRange = false;
			animator.SetBool(hash.inRangeBool, false);
		}
		
	}

	public void makeSound(AudioClip clip) {
		//ANIMATION EVENTS FOR ALL THINGS THAT NEED SOUND
		audio.clip = clip;
		audio.Play();
		
	}

}
