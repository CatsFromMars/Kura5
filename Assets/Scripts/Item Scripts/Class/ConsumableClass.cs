using UnityEngine;
using System.Collections;

public class Consumable : Item {

	public string effect; //RESTORE_LIFE or RESTORE_ENERGY
	public string preference; //ANNIE EMIL OR NONE
	public int strength; //Strength of Healing

	public Consumable(int itemID, string itemName, string itemDescription, string itemEffect, string itemPreference, int itemStrength) {
		id = itemID;
		name = itemName;
		description = itemDescription;
		effect = itemEffect;
		preference = itemPreference;
		strength = itemStrength;
	}

	public Consumable() {
		model = null;
	}

}
