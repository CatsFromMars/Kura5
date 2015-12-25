using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LensSwapTemp : MonoBehaviour {
	//Intended as a temporary fix for swapping lenses until I get a proper UI together
	public Inventory inventory;
	private bool sol = false;
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.F)) {
			int solindex = inventory.checkForLens(0);
			int fireindex = inventory.checkForLens(2);
			if(sol) {
				Debug.Log("Swapped to Sol!");
				inventory.EquipLens(solindex);
				sol = !sol;
			}
			else if(fireindex != -1) {
				Debug.Log("Swapped to Flame!");
				inventory.EquipLens(fireindex);
				sol = !sol;
			}
		}
	}
}
