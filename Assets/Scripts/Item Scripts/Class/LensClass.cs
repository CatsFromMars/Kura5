using UnityEngine;
using System.Collections;

public class Lens : Item {
	
	public string preference; //ANNIE EMIL OR NONE
	
	public Lens(int itemID, string itemName, string itemDescription, string lensPreference) {
		id = itemID;
		name = itemName;
		description = itemDescription;
		preference = lensPreference;
	}

	public Lens() {
		model = null;
	}

}
