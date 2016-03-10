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
	public int lightMax = 0;
	public bool isNightTime = false;

	//Data
	public string conditionName = "";
	public int conditionID = 0;
	public int cloudinessPercentage = 0;
	public int humidityPercentage = 0;
	public float finalTemp = 0;

	//Extremes
	public int hotTemp = 28;
	public int coldTemp = 0;
	public int humid = 80;
	public int cloudy = 60;

	void Awake() {
		InvokeRepeating("connect",0,60*60*60); //update by the hour
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
		finalTemp = GenericPattern.getTemp (percent);
		humidityPercentage = GenericPattern.getHumidity();
		cloudinessPercentage = GenericPattern.getCloudiness();
		if(percent >= 100 || percent <= 0) {
			isNightTime = true;
			lightMax = GenericPattern.getMoonlight(cloudinessPercentage);
		}
		else {
			isNightTime = false;
			lightMax = GenericPattern.getSunlight(percent, cloudinessPercentage);
		}
	}

	IEnumerator getLiveWeather() {
		//abort and go to generic weather at the sign of an error
		yield return StartCoroutine(live.requestData());

		if (live.currentError == "") {
			status = "ONLINE";
			int percent = live.getPercent();
			//Get values
			conditionName = live.getCondition();
			//conditionID = live.getID();
			finalTemp = live.getTemperature();
			humidityPercentage = live.getHumidity();
			cloudinessPercentage = live.getCloudiness();
			//calculate it it's night or day
			if(percent >= 100 || percent <= 0) {
				isNightTime = true;
				lightMax = live.getMoonlight();
			}
			else {
				isNightTime = false;
				lightMax = live.getSunlight();
			}
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