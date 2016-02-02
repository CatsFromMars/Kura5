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

	public string APIKEY = "b3abb856726ae64ca4273c826c2e4ba4";
	public string currentIP;
	public string currentCountry;
	public string currentCity;

	//Light
	public int lightMax = 0;
	public bool isNightTime = false;

	//Data
	public int cloudinessPercentage;
	public int humidityPercentage;
	public float finalTemp;

	//Status
	public int counter = 0;
	private int refreshWaitTime = 1000;
	private bool isRunning = false;
	public bool weatherSyncWorking = false;
	public string status = "";

	//retrieved from weather API
	public string retrievedCountry;
	public string retrievedCity;
	public int conditionID;
	public string conditionName;
	public string countryCode;
	public string clouds;
	public string sunriseTime;
	public string sunsetTime;
	private System.DateTime sunrise;
	private System.DateTime sunset;

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
		//Debug.Log ("restarting...");
		StartCoroutine (attemptConnect ());
	}

	IEnumerator attemptConnect() {
		StopCoroutine(SendRequest());
		StartCoroutine(SendRequest());
		while(status == "") {
			//Debug.Log ("Connecting...");
			yield return null;
		}
		if(status != "Working") getGenericWeather();
	}

	void getGenericWeather() {
		int season = GenericPattern.getSeason ();
		int percent = GenericPattern.getPercent();
		if(percent >= 100 || percent <= 0) isNightTime = true;
		else isNightTime = false;
		finalTemp = GenericPattern.getTemp (percent);
		humidityPercentage = GenericPattern.getHumidity();
		cloudinessPercentage = GenericPattern.getCloudiness ();
		lightMax = GenericPattern.getSunlight(percent, cloudinessPercentage);
	}

	public static System.DateTime UnixTimeStampToDateTime(int unixTimeStamp)
	{
		// Unix timestamp is seconds past epoch
		System.DateTime dtDateTime = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
		dtDateTime = dtDateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
		//return dtDateTime.ToString("hh:mm tt");
		return dtDateTime;
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

	public int getSunLevels() {
		int lengthOfDay = getLengthOfDay ();
		System.DateTime dt = DateTime.Now;
		int currentHour = dt.Hour;
		int currentMinute = dt.Minute;
		int sunRiseHour = sunrise.Hour;
		int sunRiseMinute = sunrise.Minute;
		sunRiseMinute += sunRiseHour * 60;
		currentMinute += currentHour * 60;

		int minutesFromSunrise = currentMinute - sunRiseMinute;

		int light = 0;
		//Debug.Log (minutesFromSunrise + ";" + lengthOfDay);
		if(minutesFromSunrise > lengthOfDay || minutesFromSunrise<=0) { //NIGHT
			light = 0; //No sun if past sunset
			isNightTime = true;
			int moonlight = MoonPhase.moonlightPercent(DateTime.Today);
			float c = (100-cloudinessPercentage)/100f;
			return Mathf.RoundToInt(c*moonlight);
		}
		else { //DAY
			isNightTime = false;
			//Curve sunlight based on a bell curve
			float percent = 100f - (minutesFromSunrise*1.0f / lengthOfDay*1.0f)*100; //Percent of the day gone
			float a = Mathf.Pow(percent-50, 2);
			float power = (-1*a/600f);
			light = Mathf.RoundToInt(10*Mathf.Exp(power));
			float c = (100-cloudinessPercentage)/100f;
			//Debug.Log (light+", "+c);
			return Mathf.RoundToInt(c*light);
		}

		return 0; //should not reach here
	}

	IEnumerator getIP() {
		WWW IPRequest = new WWW("api.ipify.org/?format=json");
		yield return IPRequest;
		
		if (IPRequest.error == null || IPRequest.error == "")
		{
			var N = JSON.Parse(IPRequest.text);
			currentIP = N["ip"].Value;
			//Debug.Log (currentIP);
		}
	}

	IEnumerator SendRequest()
	{
		//get the players IP, City, Country
		Network.Connect("http://google.com");
		Network.Disconnect();

		WWW IPRequest = new WWW("api.ipify.org/?format=json");
		yield return IPRequest;
		
		if (IPRequest.error == null || IPRequest.error == "")
		{
			var N = JSON.Parse(IPRequest.text);
			currentIP = N["ip"].Value;
		}
		
		WWW cityRequest = new WWW("http://www.geoplugin.net/json.gp?ip=" + currentIP); //get our location info
		yield return cityRequest;
		
		if (cityRequest.error == null || cityRequest.error == "")
		{
			var N = JSON.Parse(cityRequest.text);
			//Debug.Log (N);
			currentCountry = N["geoplugin_countryName"].Value;
			countryCode = N["geoplugin_countryCode"].Value;
			string regionCode = N["geoplugin_regionCode"].Value;
			string city = (N["geoplugin_city"].Value).Replace(" ", "");
			currentCity = city+","+regionCode+countryCode;
			status = "Working";
			isRunning = true;
		}
		
		else
		{
			Debug.LogError("WWW error: " + cityRequest.error);
			status = "Internet problem";
			isRunning = false;
		}

		//Spoofing
		if(spoofMode) {
			currentCity = spoofCity;
			currentCountry = spoofCountry;
		}

		//get the current weather
		WWW request = new WWW("http://api.openweathermap.org/data/2.5/weather?q=" + currentCity + "&APPID=" + APIKEY); //get our weather
		yield return request;
		Debug.Log (currentCity);
		
		if (request.error == null || request.error == "")
		{
			var N = JSON.Parse(request.text);
			status = "Working";
			isRunning = true;
			Debug.Log (N);

			retrievedCountry = N["sys"]["country"].Value; //get the country
			retrievedCity = N["name"].Value; //get the city
			sunrise = (UnixTimeStampToDateTime(int.Parse(N["sys"]["sunrise"])));
			sunset = (UnixTimeStampToDateTime(int.Parse(N["sys"]["sunset"])));

			string temp = N["main"]["temp"].Value; //get the temperature
			float tempTemp; //variable to hold the parsed temperature
			float.TryParse(temp, out tempTemp); //parse the temperature
			finalTemp = Mathf.Round(((tempTemp - 273.0f)*10)/10); //holds the actual converted temperature
			
			int.TryParse(N["weather"][0]["id"].Value, out conditionID); //get the current condition ID
			conditionName = N["weather"][0]["main"].Value; //get the current condition Name
			//conditionName = N["weather"][0]["description"].Value; //get the current condition Description
			clouds = N["clouds"]["all"].Value;
			float tempCloud;
			float.TryParse(clouds, out tempCloud);
			cloudinessPercentage = Mathf.RoundToInt(tempCloud);

			string humidity = N["main"]["humidity"].Value; //get the humitity
			float tempHumidity;
			float.TryParse(humidity, out tempHumidity);
			humidityPercentage = Mathf.RoundToInt(tempHumidity);

			//Update Light Levels
			lightMax = getSunLevels();

			//Update weather effects in a room
//			Debug.Log (System.DateTime.Now);
//			GameObject skylight = GameObject.FindGameObjectWithTag("Sunlight");
//			if(skylight != null) {
//				SkylightFade s = skylight.GetComponent<SkylightFade>();
//				if(s != null) s.updateSkylightColor();
//		}
			//Adjust Max Sun
			GameObject lightLevels = GameObject.FindGameObjectWithTag("LightLevels");
			if(lightLevels != null) lightLevels.GetComponent<LightLevels>().upperBound = lightMax+2;
		}
		else
		{
			Debug.LogError("WWW error: " + request.error);
			status = "Internet problem";
			isRunning = false;
		}

		if (currentCity == "") {
			Debug.LogError("Weather Sync Failed at IP " + currentIP);

		}

	}
}