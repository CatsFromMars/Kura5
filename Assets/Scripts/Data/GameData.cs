using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

[System.Serializable]
public class GameData : MonoBehaviour {

	//NOTE: ADD SKILL POINTS AND LEVEL UPS LATER

	public bool nearInteractable = false; //For treasure chests and dialogue and stuff

	//PLAYER DATA: ANNIE
	public float annieCurrentLife = 100;
	public float annieCurrentEnergy = 100;
	public float annieMaxEnergy = 100;
	public float annieMaxLife = 100;
	public elementalProperty annieCurrentElem = elementalProperty.Sol;

	//PLAYER DATA: EMIL
	public float emilCurrentLife;
	public float emilCurrentEnergy;
	public float emilMaxEnergy;
	public float emilMaxLife;
	public elementalProperty emilCurrentElem = elementalProperty.Dark;

	//PLAYER DATA: GLOBAL
	public enum player 
	{
		Annie, Emil
	}
	public player currentPlayer = player.Annie;

	//LEVEL DATA: USEFUL FOR LOADING SAVES
	public Vector3 lastCheckpoint = Vector3.zero; //To be altered by SceneTransition.cs
	public string sceneName; //NAME OF DUNGEON. WILL BE LOADED AS "Application.LoadLevel(sceneName);"

	//MISC VARIABLES
	public Transform gameOverSpawner;
	public bool isGameOver;

	//ELEMENT
	public enum elementalProperty 
	{
		Sol, Dark, Fire, Frost, Cloud, Earth, Null
	}

	void Awake() {
		Application.targetFrameRate = 60;
	}

	void Update() {

		//CONSTRAINTS
		if(annieCurrentLife >= annieMaxLife) annieCurrentLife = annieMaxLife;
		if(emilCurrentLife >= emilMaxLife) emilCurrentLife = emilMaxLife;
		if(annieCurrentEnergy >= annieMaxEnergy) annieCurrentEnergy = annieMaxEnergy;
		if(emilCurrentEnergy >= emilMaxEnergy) emilCurrentEnergy = emilMaxEnergy;



		//Check for Game Over
		if (annieCurrentLife <= 0 && emilCurrentLife <= 0 && !isGameOver) {
			isGameOver = true;
			Instantiate(Resources.Load ("Effects/GameOver"), gameOverSpawner.position, Quaternion.Euler(45,0,0));
		}

	}


}
