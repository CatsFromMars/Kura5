using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

[System.Serializable]
public class GameData : MonoBehaviour {

	//NOTE: ADD SKILL POINTS AND LEVEL UPS LATER

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
	public bool isTalking;
	public enum player 
	{
		Annie, Emil
	}
	public player currentPlayer = player.Annie;
	public Transform anniePrefab; //ANNIE'S PREFAB
	public Transform emilPrefab; //EMIL'S PREFAB
	Transform dungeonEnterencePoint;
	public Transform thePlayer;

	//LEVEL DATA: USEFUL FOR LOADING SAVES
	public Vector3 playerLocation;
	public Vector3 playerCurrentPosition;
	public string sceneName; //NAME OF DUNGEON. WILL BE LOADED AS "Application.LoadLevel(sceneName);"

	//MISC VARIABLES
	public bool inCutscene = false; //TO BE MESSED WITH BY CUTSCENE SCRIPT

	//ELEMENT
	public enum elementalProperty 
	{
		Sol, Dark, Fire, Frost, Cloud, Earth, Null
	}

	void Awake() {
		Application.targetFrameRate = 300;
	}
	
	void Update() {

		//CONSTRAINTS
		if(annieCurrentLife >= annieMaxLife) annieCurrentLife = annieMaxLife;
		if(emilCurrentLife >= emilMaxLife) emilCurrentLife = emilMaxLife;
		if(annieCurrentEnergy >= annieMaxEnergy) annieCurrentEnergy = annieMaxEnergy;
		if(emilCurrentEnergy >= emilMaxEnergy) emilCurrentEnergy = emilMaxEnergy;



		//Check for Game Over
		//if(annieCurrentLife <= 0 && emilCurrentLife <= 0)

	}


}
