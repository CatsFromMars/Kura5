using System;
using UnityEngine;
using System.Collections;
using SimpleJSON;

public class WeatherSync : MonoBehaviour {
	//Debug vars REMOVE UPON RELEASE
	public bool spoofMode = false;
	public string spoofCity = "";
	public string spoofCountry = "";
	//End debug

	//Script reference to live weather
	public bool weatherActivated = true;
	public LiveWeather live;
	public string status = "OFFLINE";

	//Light
	public SafeInt sunlight = new SafeInt(0);
	public SafeInt lightMax = new SafeInt(0);
	public bool isNightTime = false;

	//Data
	public string conditionName = "";
	public SafeInt conditionID = new SafeInt(-1000);
	public SafeInt cloudinessPercentage = new SafeInt(-1000);
	public SafeInt humidityPercentage = new SafeInt(-1000);
	public SafeInt finalTemp = new SafeInt(-1000);

	//Extremes
	public SafeInt hotTemp = new SafeInt(28);
	public SafeInt coldTemp = new SafeInt(0);
	public SafeInt humid = new SafeInt(80);
	public SafeInt cloudy = new SafeInt(60);

	void Awake() {
		InvokeRepeating("connect",0,60*60*60); //update by the hour
		//InvokeRepeating("adjustLight",0,30);
		//connect ();
	}
	
	void OnLevelWasLoaded(int level) {
		//connect();
	}

	void connect() {
		if(weatherActivated) StartCoroutine (getLiveWeather());
		else getGenericWeather();
	}

	void getGenericWeather() {
		conditionName = ""; //Add in later
		int season = GenericPattern.getSeason();
		int percent = GenericPattern.getPercent();
		finalTemp = new SafeInt(GenericPattern.getTemp (percent));
		humidityPercentage = new SafeInt(GenericPattern.getHumidity());
		cloudinessPercentage = new SafeInt(GenericPattern.getCloudiness());
		if(percent >= 100 || percent <= 0) {
			isNightTime = true;
			lightMax = new SafeInt(GenericPattern.getMoonlight(cloudinessPercentage.GetValue()));
		}
		else {
			isNightTime = false;
			lightMax = new SafeInt(GenericPattern.getSunlight(percent, cloudinessPercentage.GetValue()));
		}
		sunlight = lightMax;
	}

	void adjustLight() {
		//Adjusts light at random according to cloudiness
		if(conditionName != "") {
			int r = UnityEngine.Random.Range(0, 100);
			int cloud = 100-cloudinessPercentage.GetValue()/2;
			if(r <= (cloud)) {
				lightMax = new SafeInt(sunlight.GetValue()+UnityEngine.Random.Range(0,3));
			}
			else if(r >= cloud) {
				lightMax = new SafeInt(sunlight.GetValue()-UnityEngine.Random.Range(0,1));
			}
			if(lightMax > 10) lightMax = new SafeInt(10);
			if(lightMax < 0) lightMax = new SafeInt(0);
			//Debug.Log("LightMax went from "+sunlight+" to "+lightMax);
		}
	}

	IEnumerator getLiveWeather() {
		//abort and go to generic weather at the sign of an error
		yield return StartCoroutine(live.requestData());

		if (live.currentError == "") {
			status = "ONLINE";
			SafeInt percent = live.getPercent();
			//Get values
			conditionName = live.getCondition();
			conditionID = live.getID();
			finalTemp = live.getTemperature();
			humidityPercentage = live.getHumidity();
			cloudinessPercentage = live.getCloudiness();
			//calculate it it's night or day
			if(percent.GetValue() >= 100 || percent.GetValue() <= 0) {
				isNightTime = true;
				lightMax = live.getMoonlight();
			}
			else {
				isNightTime = false;
				lightMax = live.getSunlight();
			}

			sunlight = lightMax;
		}
		else {
			getGenericWeather();
			status = "OFFLINE";
			Debug.Log (live.currentError+": Shifting to generic weather");
		}
	}

	bool checkForCheating() {
		return false;
	}
	
}