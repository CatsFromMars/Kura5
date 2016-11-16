using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

[System.Serializable]
public class GameData : MonoBehaviour {

	//NOTE: ADD SKILL POINTS AND LEVEL UPS LATER

	public bool nearInteractable = false; //For treasure chests and dialogue and stuff
	private Transform globalObject;

	//PLAYER DATA: ANNIE
	public bool canSwapToAnnie = true;
	public float annieCurrentLife = 100;
	public float annieCurrentEnergy = 100;
	public float annieMaxEnergy = 100;
	public float annieMaxLife = 100;
	public elementalProperty annieCurrentElem = elementalProperty.Sol;

	//PLAYER DATA: EMIL
	public bool canSwapToEmil = false;
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
	public Vector3 lastDungeonPos = Vector3.zero; //To be altered by SceneTransition.cs
	public Vector3 lastDungeonScene; //To be altered by SceneTransition.cs
	public Vector3 lastCheckpoint = Vector3.zero; //To be altered by SceneTransition.cs
	public string sceneName; //NAME OF DUNGEON. WILL BE LOADED AS "Application.LoadLevel(sceneName);"

	//MISC VARIABLES
	public Transform emilRaven;
	public Transform gameOverSpawner;
	public bool isGameOver;

	//BANK VARIABLES
	public int bankSoll = 500;

	//ELEMENT
	public enum elementalProperty 
	{
		Sol, Dark, Fire, Frost, Cloud, Earth, Null
	}

	void Awake() {
		Application.targetFrameRate = 60;
		GameObject GO = GameObject.FindGameObjectWithTag ("Global");
		if(GO!=null) globalObject = GO.transform;
	}

	void Update() {

		//CONSTRAINTS
		if(annieCurrentLife >= annieMaxLife) annieCurrentLife = annieMaxLife;
		if(emilCurrentLife >= emilMaxLife) emilCurrentLife = emilMaxLife;
		if(annieCurrentEnergy >= annieMaxEnergy) annieCurrentEnergy = annieMaxEnergy;
		if(emilCurrentEnergy >= emilMaxEnergy) emilCurrentEnergy = emilMaxEnergy;

	}



}
