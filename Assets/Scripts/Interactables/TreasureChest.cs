using UnityEngine;
using System.Collections;

public class TreasureChest : MonoBehaviour {

	Animator animator;
	GameObject gameData;
	//GameData Data;
	HashIDs hash;

	public Transform loot; //ITEM THAT CHEST IS SUPPOSED TO CONTAIN!
	bool itemSpawned = false;

	// Use this for initialization
	void Awake() {
		gameData = GameObject.FindGameObjectWithTag("GameController");
		animator = GetComponent<Animator>();
		//Data = gameData.GetComponent<GameData>();
		hash = gameData.GetComponent<HashIDs>();
	}
	
	void OnTriggerStay(Collider other) {
		if (other.tag == "Player") {
			if(Input.GetButtonDown("Confirm")) {
				animator.SetBool (hash.unlockedBool, true);
				//audio.Play();
			}
		}
		
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			animator.SetBool (hash.unlockedBool, false);
			//audio.Play();
		}
	}

	void spawnLoot() {
		//ANIMATION EVENT
		if(!itemSpawned) {
			Instantiate (loot, transform.position, Quaternion.Euler(-90,0,0));
			itemSpawned = true;
		}
	}

	void DestroySelf() {
		Destroy (this.gameObject);
	}

}
