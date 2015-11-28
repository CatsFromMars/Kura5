using UnityEngine;
using System.Collections;

public class Saving : MonoBehaviour {
	public GameData gameData;

	public void saveAll() {
		
	}

	public void saveOptions() {
		ES2.Save(transform, "file.txt?tag=trTag");
	}

	public void saveGameData() {
		//Save Annie's current LIFE
		//Save Emil's current LIFE
		//Save Annie's current ENERGY
		//Save Emil's curent ENERGY
	}

	public void saveInventory() {

	}

	public void saveStoryFlags() {

	}

	public void loadAll() {

	}

	public void loadOptions() {

	}

}
