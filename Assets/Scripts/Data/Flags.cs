using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flags : MonoBehaviour {
	//Saves flags for traps and cutscenes
	//Should never be with global data
	//Assumes ONE trap per scene

	public Dictionary<string, bool> traps;

	void Awake() {
		traps = new Dictionary<string, bool> ();
	}

	//Traps
	public void AddTrapFlag() {
		//Add trap flag on awake from trap script IF trap not added yet
		//Trap NOT cleared by default
		if(!traps.ContainsKey(Application.loadedLevelName)) traps.Add(Application.loadedLevelName, false);
		Debug.Log (traps [Application.loadedLevelName]);
	}

	public void SetTrapToCleared() {
		//Works because only one trap allowed per scene
		traps [Application.loadedLevelName] = true;
	}

	public bool CheckTrapFlag() {
		return traps [Application.loadedLevelName];
	}
}
