using UnityEngine;
using System.Collections;

public class Lens : Item {
	
	public string preference; //ANNIE EMIL OR NONE
	public string element;
	
	public Lens(int itemID, string itemName, string itemDescription, string lensPreference, string lensElement) {
		id = itemID;
		name = itemName;
		description = itemDescription;
		preference = lensPreference;
		element = lensElement;
	}

	public Lens() {
		model = null;
	}

}
