using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameState{ 
	
	public static GameState current;
	//Flags
	public Dictionary<string, bool> traps;
	public Dictionary<string, bool> cutscenes;
	public Dictionary<string, bool> treasurechests;
	public Dictionary<string, bool> doors;
	public Dictionary<string, bool> other;
	//Weather
	public int weatherTemplate;
	//Player Stats
	public float annieLife;
	public float annieEnergy;
	public float emilLife;
	public float emilEnergy;
	public bool canSwapToEmil;
	public bool canSwapToAnnie;
	//Bank States
	public int bankSoll;
	//Location
	public float playerLocationX;
	public float playerLocationY;
	public float playerLocationZ;
	public string currentScene;
	//Inventory
	public int[] itemsList;
	public int[] keyItems;
	public int[] lensList;
	
	public GameState () {
		//Flags
		Flags flags = GetUtil.getFlags();
		traps = flags.traps;
		cutscenes = flags.cutscenes;
		treasurechests = flags.treasurechests;
		doors = flags.doors;
		other = flags.other;

		//Game Data
		GameData data = GetUtil.getData();
		Inventory inventory = GetUtil.getInventory ();
		annieLife = data.annieCurrentLife;
		emilLife = data.emilCurrentLife;
		annieEnergy = data.annieCurrentEnergy;
		emilEnergy = data.emilCurrentEnergy;
		canSwapToAnnie = data.canSwapToAnnie;
		canSwapToEmil = data.canSwapToEmil;
		currentScene = data.sceneName;
		playerLocationX = data.lastCheckpoint.x;
		playerLocationY = data.lastCheckpoint.y;
		playerLocationZ = data.lastCheckpoint.z;
		//Bank
		bankSoll = data.bankSoll;
		//Save Inventory
		itemsList = inventory.getInventory();
		keyItems = inventory.getKeyItems();
		lensList = inventory.getLenses();
		//Save Weather
		WeatherSync w = GetUtil.getWeather ();
		weatherTemplate = w.template;
	}
	
}