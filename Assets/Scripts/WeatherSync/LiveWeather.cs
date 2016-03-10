﻿using System;
using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;

public class LiveWeather : MonoBehaviour {
	//API Key
	private string APIKEY = "dc9a8ccf17ac92f754c1755832360a71";
	//Data
	private string currentIP;
	public string currentError;
	private string locationData;

	private int temperature;
	private int humidity;
	private int cloudiness;
	private int conditionID;
	private string name;

	private string conditionName;
	private System.DateTime sunrise;
	private System.DateTime sunset;
	private string currentLocation; //String for feeding into weatherSync

	void Awake() {
		currentError = "";
	}

	public static System.DateTime UnixTimeStampToDateTime(int unixTimeStamp)
	{
		// Unix timestamp is seconds past epoch
		System.DateTime dtDateTime = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
		dtDateTime = dtDateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
		//return dtDateTime.ToString("hh:mm tt");
		return dtDateTime;
	}

	IEnumerator getIP() {
		WWW IPRequest = new WWW("api.ipify.org/?format=json");
		yield return IPRequest;
		if (IPRequest.error == null || IPRequest.error == "")
		{
			var N = JSON.Parse(IPRequest.text);
			currentIP = N["ip"].Value;		
		}
		else {
			currentError = "IP error: " + IPRequest.error;
		}
	}
	

	IEnumerator getLocation() {
		WWW cityRequest = new WWW("http://www.geoplugin.net/json.gp?ip=" + currentIP); //get our location info
		yield return cityRequest;
		if (cityRequest.error == null || cityRequest.error == "")
		{
			var N = JSON.Parse(cityRequest.text);
			string code = N["geoplugin_countryCode"].Value;
			string country = N["geoplugin_countryName"].Value;
			string region = N["geoplugin_regionCode"].Value;
			string city = (N["geoplugin_city"].Value).Replace(" ", "");
			currentLocation = city+","+region+","+code;
			Debug.Log(currentLocation);
		}
		else {
			currentError = "Location error: " + cityRequest.error;
		}
	}

	IEnumerator getLiveWeather() {
		WWW weatherRequest = new WWW("http://api.openweathermap.org/data/2.5/weather?q=" + currentLocation + "&units=metric" + "&APPID=" + APIKEY); //get our weather
		//Debug.Log (weatherRequest.url);
		yield return weatherRequest;
		if (weatherRequest.error == null || weatherRequest.error == "")
		{
			var N = JSON.Parse(weatherRequest.text);
			conditionName = N["weather"][0]["main"].Value;
			conditionID = ParseUtil.numberParse(N["weather"][0]["id"].Value);
			temperature = Mathf.RoundToInt(ParseUtil.numberParse(N["main"]["temp"].Value));
			cloudiness = ParseUtil.numberParse(N["clouds"]["all"].Value);
			humidity = ParseUtil.numberParse(N["main"]["humidity"].Value);
			sunrise = UnixTimeStampToDateTime(ParseUtil.numberParse(N["sys"]["sunrise"]));
			sunset = UnixTimeStampToDateTime(ParseUtil.numberParse(N["sys"]["sunset"]));
		}
		else {
			currentError = "Weather error: " + weatherRequest.error;
		}
	}

	public int getLengthOfDay() {
		//Returns Length of day in minutes
		int sunSetHour = sunset.Hour;
		int sunSetMinute = sunset.Minute;
		int sunRiseHour = sunrise.Hour;
		int sunRiseMinute = sunrise.Minute;
		
		sunRiseMinute += sunRiseHour * 60;
		sunSetMinute += sunSetHour * 60;
		return sunSetMinute - sunRiseMinute;
	}

	public int getHumidity() {
		return humidity;
	}

	public int getCloudiness() {
		return cloudiness;
	}

	public int getTemperature() {
		return temperature;
	}

	public int getID() {
		return conditionID;
	}

	public int getPercent() {
		//Percent of day
		int lengthOfDay = getLengthOfDay();
		System.DateTime dt = DateTime.Now;
		int currentHour = dt.Hour;
		int currentMinute = dt.Minute;
		int sunRiseHour = sunrise.Hour;
		int sunRiseMinute = sunrise.Minute;
		sunRiseMinute += sunRiseHour * 60;
		currentMinute += currentHour * 60;
		
		int minutesFromSunrise = currentMinute - sunRiseMinute;
		float percent = 100f - (minutesFromSunrise*1.0f / lengthOfDay*1.0f)*100;
		return Mathf.RoundToInt(percent);
	}

	public static int normalize(int percentage) {
		//Returns a number between 1 and 10
		float a = Mathf.Pow(percentage-50, 2);
		float power = (-1*a/600f);
		return Mathf.RoundToInt(10*Mathf.Exp(power));
	}

	public int getSunlight() {
		int percent = getPercent();
		int cloudiness = getCloudiness();
		float c = (100-cloudiness)/100f;
		int sun = Mathf.RoundToInt(normalize(percent)*c);
		if(sun<0) sun = 0;
		return sun;
	}

	public string getCondition() {
		return conditionName;
	}

	public int getMoonlight() {
		int cloudiness = getCloudiness();
		float percent = MoonPhase.moonlightPercent(DateTime.Today) - cloudiness/10f;
		return Mathf.RoundToInt(percent);
	}

	public IEnumerator requestData() {
		yield return StartCoroutine(getIP());
		if(currentError == "") yield return StartCoroutine(getLocation());
		if(currentError == "") yield return StartCoroutine(getLiveWeather());
	}

}
