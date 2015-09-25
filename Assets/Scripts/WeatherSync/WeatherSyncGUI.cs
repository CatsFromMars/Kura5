using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeatherSyncGUI : MonoBehaviour {
	public WeatherSync w;
	Text text;
	// Use this for initialization
	void Awake () {
		text = GetComponent<Text>();
	}

	void Update() {
		text.text = ("Status: " + w.status
		 + "\nTime: " + System.DateTime.Now.ToString("hh:mm tt")
		 + "\nCity: " + w.retrievedCity
		 + "\nTemp: " + w.finalTemp + " C"
		 + "\nWeather: " + w.conditionName
		 + "\nSunset: " + w.sunsetTime
		 + "\nSunrise: " + w.sunriseTime);
	}
}
