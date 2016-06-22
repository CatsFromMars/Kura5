using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flags : MonoBehaviour {
	//Saves flags for traps and cutscenes
	//Should never be with global data
	//Assumes ONE trap per scene

	public Dictionary<string, bool> traps;
	public Dictionary<string, bool> cutscenes;

	void Awake() {
		traps = new Dictionary<string, bool>();
		cutscenes = new Dictionary<string, bool>();
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
}
