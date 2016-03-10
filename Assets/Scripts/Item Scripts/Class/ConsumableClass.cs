using UnityEngine;
using System.Collections;
using System;

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

	public int CompareTo(Consumable other)
	{
		if(other == null)
		{
			return 1;
		}
		
		//SORTS BY ID NUMBER
		return id - other.id;
	}

}
