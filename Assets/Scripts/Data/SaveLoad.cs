using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

public static class SaveLoad {

	public static GameState state;

	public static void Save() {
		SaveLoad.state = new GameState(); //Should load the data
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.dataPath + "/save.bok");
		bf.Serialize(file, SaveLoad.state);
		file.Close();
	}

	public static bool Load() {
		if(File.Exists(Application.dataPath + "/save.bok")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.dataPath + "/save.bok", FileMode.Open);
			SaveLoad.state = (GameState)bf.Deserialize(file); //Set the game state
			file.Close();
			//Actually load the game
			SaveLoad.loadGameData();
			return true;
		}
		else return false;
	}

	public static void loadGameData() {
		//Data
		GameData data = GetUtil.getData();
		data.annieCurrentLife = SaveLoad.state.annieLife;
		data.annieCurrentEnergy = SaveLoad.state.annieEnergy;
		data.emilCurrentLife = SaveLoad.state.emilLife;
		data.emilCurrentEnergy = SaveLoad.state.emilEnergy;
		data.canSwapToEmil = SaveLoad.state.canSwapToEmil;
		data.canSwapToAnnie = SaveLoad.state.canSwapToAnnie;
		//Flags
		Flags flags = GetUtil.getFlags ();
		flags.cutscenes = SaveLoad.state.cutscenes;
		flags.doors = SaveLoad.state.doors;
		flags.traps = SaveLoad.state.traps;
		flags.treasurechests = SaveLoad.state.treasurechests;
		flags.other = SaveLoad.state.other;
		//Inventory
		Inventory inventory = GetUtil.getInventory();
		inventory.loadInventory (state.itemsList);
		inventory.loadKeyItems (state.keyItems);
		inventory.loadLenses (state.lensList);
		//Weather
		GetUtil.getWeather().setWeatherTemplate(state.weatherTemplate);
	}
	
}