using UnityEngine;
using System.Collections;

public class CharacterSwapper : MonoBehaviour {
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
		if(Input.GetButtonDown("Switch")) switchPlayers();
		if (data.currentPlayer == GameData.player.Annie)
						transform.position = annie.position;
		else transform.position = emil.position;
	}

	void switchPlayers() {
		if(data.currentPlayer == GameData.player.Annie && data.emilCurrentLife > 0) {
			Vector3 spawnPoint = annie.transform.position;
			annie.active = false;
			emil.active = true;
			emil.transform.position = spawnPoint;
			data.currentPlayer = GameData.player.Emil;
		}
		else if(data.currentPlayer == GameData.player.Emil && data.annieCurrentLife > 0) {
			Vector3 spawnPoint = emil.transform.position;
			emil.active = false;
			annie.active = true;
			annie.transform.position = spawnPoint;
			data.currentPlayer = GameData.player.Annie;
		}
		
	}
}
