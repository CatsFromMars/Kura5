using UnityEngine;
using System.Collections;

public class DamageCalculator : MonoBehaviour {

	GameObject globalData;
	ElementDatabase elements;
	WeatherSync w;

	// Use this for initialization
	void Awake() {
		globalData = GameObject.FindGameObjectWithTag("GameController");
		elements = globalData.GetComponent<ElementDatabase>();
		w = GameObject.FindGameObjectWithTag("Weather").GetComponent<WeatherSync>();
	}

	float calculateMultipler(string element) {
		if(element == "Null") {
			return 0.8f;
		}
		else if (element == "Dark") {
			if(w.isNightTime && w.lightMax.GetValue() >= 5) return 1.2f;
			else if(w.isNightTime==false && w.lightMax.GetValue() >= 5) return 0.8f;
			else return 1;
		}
		else if (element == "Sol") {
			if(w.isNightTime==false && w.lightMax.GetValue() >= 5) return 1.2f;
			else if(w.isNightTime && w.lightMax.GetValue() >= 5) return 0.8f;
			else return 1;
		}
		else if (element == "Fire") {
			if(w.finalTemp >= w.hotTemp) return 1.2f;
			else if(w.finalTemp <= w.coldTemp) return 0.8f;
			else return 1;
		}
		else if (element == "Frost") {
			if(w.finalTemp <= w.coldTemp) return 1.2f;
			else if(w.finalTemp >= w.hotTemp) return 0.8f;
			else return 1;
		}
		else if (element == "Cloud") {
			if(w.cloudinessPercentage >= w.cloudy) return 1.2f;
			else if(w.humidityPercentage >= w.humid) return 0.8f;
			else return 1;
		}
		else if (element == "Earth") {
			if(w.humidityPercentage >= w.humid) return 1.2f;
			else if(w.cloudinessPercentage >= w.cloudy) return 0.8f;
			else return 1;
		}
		else if (element == "Luna") {
			return 0;
		}
		
		else return 1;
	}
	
	public int getDamage(string elementTarget, string elementSelf, int baseDamage, float m=1f) {
		//target = thing that damage is being applied to
		//Source = source of attack
		m = calculateMultipler(elementTarget);
		Element e1 = getElementFromString (elementTarget);
		Element e2 = getElementFromString (elementSelf);
		Attack atk = new Attack (baseDamage, e1, m);
		return atk.calculateDamage(e2);
	}

	public Element getElementFromString(string e) {
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
