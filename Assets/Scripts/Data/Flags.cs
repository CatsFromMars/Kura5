using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flags : MonoBehaviour {
	//Saves flags for traps and cutscenes
	//Should never be with global data
	//Assumes ONE trap per scene

	public Dictionary<string, bool> traps;
	public Dictionary<string, bool> cutscenes;
	public Dictionary<Vector3, bool> treasurechests;
	public Dictionary<Vector3, bool> doors;

	void Awake() {
		traps = new Dictionary<string, bool>();
		cutscenes = new Dictionary<string, bool>();
		treasurechests = new Dictionary<Vector3, bool>();
		doors = new Dictionary<Vector3, bool>();
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
		if(!treasurechests.ContainsKey(chest)) treasurechests.Add(chest, false);
	}
	
	public void SetTreasureToOpen(Vector3 chest) {
		treasurechests[chest] = true;
	}
	
	public bool CheckTreasureFlag(Vector3 chest) {
		return treasurechests[chest];
	}
	//Locked Doors
	public void AddDoorFlag(Vector3 door) {
		if(!doors.ContainsKey(door)) doors.Add(door, false);
	}
	
	public void SetDoorToOpen(Vector3 door) {
		doors[door] = true;
	}
	
	public bool CheckDoorFlag(Vector3 chest) {
		return doors[chest];
	}
}
