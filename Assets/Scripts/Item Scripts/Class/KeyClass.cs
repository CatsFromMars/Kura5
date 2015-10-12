using UnityEngine;
using System.Collections;

public class KeyItem : Item {
	
	public bool canBeUsed; 
	public string effect;
	
	public KeyItem(int itemID, string itemName, string itemDescription, bool canUse, string itemEffect) {
		id = itemID;
		name = itemName;
		description = itemDescription;
		effect = itemEffect;
		canBeUsed = canUse;
	}

	public KeyItem() {
		model = null;
	}

	
}
