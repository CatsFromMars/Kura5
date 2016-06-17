using UnityEngine;
using System.Collections;

public class CharacterSwapper : MonoBehaviour {
	public ElementSwapping ele;
	GameObject globalData;
	GameData data;
	public Transform annie;
	public Transform emil;

	// Use this for initialization
	void Awake() {
		globalData = GameObject.FindGameObjectWithTag("GameController");
		data = globalData.GetComponent<GameData>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Switch")) {
			PlayerContainer player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerContainer>();
			bool inControl = player.playerInControl;
			bool canSwitch = player.currentAnim(player.hash.idleState) && !player.inCoffin && Time.timeScale!=0;
			if(inControl&&canSwitch) switchPlayers();
		}
		if (data.currentPlayer == GameData.player.Annie)
						transform.position = annie.position;
		else transform.position = emil.position;
	}

	public void displayBoth() {
		//displays both characters at once
		emil.active = true;
		annie.active = true;
		if(data.currentPlayer == GameData.player.Annie) emil.transform.position = emil.transform.position+(emil.transform.forward*2);
		if(data.currentPlayer == GameData.player.Emil) annie.transform.position = annie.transform.position+(annie.transform.forward*2);
	}

	public void hideInactivePlayer() {
		if(data.currentPlayer == GameData.player.Annie && data.emilCurrentLife > 0) {
			Vector3 spawnPoint = annie.transform.position;
			emil.transform.position = spawnPoint;
			annie.active = true;
			emil.active = false;
		}
		else if(data.currentPlayer == GameData.player.Emil && data.annieCurrentLife > 0) {
			Vector3 spawnPoint = emil.transform.position;
			annie.transform.position = spawnPoint;
			emil.active = true;
			annie.active = false;
		}
	}

	public void switchPlayers() {
		if(data.currentPlayer == GameData.player.Annie && data.emilCurrentLife > 0) {
			Vector3 spawnPoint = annie.transform.position;
			emil.transform.position = spawnPoint;
			annie.active = false;
			emil.active = true;

			data.currentPlayer = GameData.player.Emil;
		}
		else if(data.currentPlayer == GameData.player.Emil && data.annieCurrentLife > 0) {
			Vector3 spawnPoint = emil.transform.position;
			annie.transform.position = spawnPoint;
			emil.active = false;
			annie.active = true;

			data.currentPlayer = GameData.player.Annie;
		}

		if(ele!=null) ele.updateList();
		
	}
}
