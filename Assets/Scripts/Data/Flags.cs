using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flags : MonoBehaviour {
	//Saves flags for traps and cutscenes
	//Should never be with global data
	//Assumes ONE trap per scene

	public Dictionary<string, bool> traps;
	public Dictionary<string, bool> cutscenes;
	public Dictionary<string, bool> treasurechests;
	public Dictionary<string, bool> doors;
	public Dictionary<string, bool> other; //Use for puzzles and weird story flags
	public int immortalHeartsEaten = 0;

	void Awake() {
		traps = new Dictionary<string, bool>();
		cutscenes = new Dictionary<string, bool>();
		treasurechests = new Dictionary<string, bool>();
		doors = new Dictionary<string, bool>();
		other = new Dictionary<string, bool>();
	}
	
	//Cutscenes
	public void AddCutsceneFlag(string d) {
		if(!cutscenes.ContainsKey(d)) cutscenes.Add(d, false);
	}

	public void SetCutscene(string d) {
		//Works because only one trap allowed per scene
		cutscenes[d] = true;
	}

	public bool CheckCutsceneFlag(string d) {
		if(cutscenes.ContainsKey(d)) return cutscenes[d];
		else return false;
	}

	//Traps
	public void AddTrapFlag() {
		//Add trap flag on awake from trap script IF trap not added yet
		//Trap NOT cleared by default
		if(!traps.ContainsKey(Application.loadedLevelName)) traps.Add(Application.loadedLevelName, false);
	}

	public void SetTrapToCleared() {
		//Works because only one trap allowed per scene
		traps [Application.loadedLevelName] = true;
	}

	public bool CheckTrapFlag() {
		return traps [Application.loadedLevelName];
	}

	//Treasure Chests
	public void AddTreasureFlag(Vector3 chest) {
		float pos = chest.x + chest.y + chest.z;
		string key = Application.loadedLevelName + pos.ToString();
		if(!treasurechests.ContainsKey(key)) treasurechests.Add(key, false);
	}
	
	public void SetTreasureToOpen(Vector3 chest) {
		float pos = chest.x + chest.y + chest.z;
		string key = Application.loadedLevelName + pos.ToString();
		treasurechests[key] = true;
	}
	
	public bool CheckTreasureFlag(Vector3 chest) {
		float pos = chest.x + chest.y + chest.z;
		string key = Application.loadedLevelName + pos.ToString();
		return treasurechests[key];
	}
	//Locked Doors
	public void AddDoorFlag(GameObject door) {
		float pos = door.transform.position.x + door.transform.position.y + door.transform.position.z;
		string key = Application.loadedLevelName + pos.ToString();
		if(!doors.ContainsKey(key)) doors.Add(key, false);
	}
	
	public void SetDoorToOpen(GameObject door) {
		float pos = door.transform.position.x + door.transform.position.y + door.transform.position.z;
		string key = Application.loadedLevelName + pos.ToString();
		doors[key] = true;
	}
	
	public bool CheckDoorFlag(GameObject door) {
		float pos = door.transform.position.x + door.transform.position.y + door.transform.position.z;
		string key = Application.loadedLevelName + pos.ToString();
		return doors[key];
	}

	//Other
	public void AddOtherFlag(string d) {
		if(!other.ContainsKey(d)) other.Add(d, false);
	}
	
	public void SetOther(string d) {
		//Works because only one trap allowed per scene
		other[d] = true;
	}
	
	public bool CheckOtherFlag(string d) {
		if(other.ContainsKey(d)) return other[d];
		else return false;
	}

	public void setImmortalHearts() {
		immortalHeartsEaten++;
	}

}
