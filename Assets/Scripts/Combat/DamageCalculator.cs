using UnityEngine;
using System.Collections;

public class DamageCalculator : MonoBehaviour {

	GameObject globalData;
	ElementDatabase elements;

	// Use this for initialization
	void Awake() {
		globalData = GameObject.FindGameObjectWithTag("GameController");
		elements = globalData.GetComponent<ElementDatabase>();
	}
	
	public int getDamage(string elementTarget, string elementSelf, int baseDamage, int m=1) {
		//target = thing that damage is being applied to
		//Source = source of attack
		Element e1 = getElementFromString (elementTarget);
		Element e2 = getElementFromString (elementSelf);
		Attack atk = new Attack (baseDamage, e1, m);
		return atk.calculateDamage(e2);
	}

	Element getElementFromString(string e) {
		if(e == "Sol") return elements.Sol;
		else if(e == "Dark") return elements.Dark;
		else if(e == "Fire") return elements.Fire;
		else if(e == "Frost") return elements.Frost;
		else if(e == "Cloud") return elements.Cloud;
		else if(e == "Earth") return elements.Earth;
		else if(e == "Null") return elements.Null;
		else {
			Debug.LogError("INVALID ELEMENT EVALUATION!" + e);
			return null;
		}
	}
}
