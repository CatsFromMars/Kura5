using UnityEngine;
using System.Collections;

public class SkylightWeather : MonoBehaviour {

	public bool isIndoors = true;

	private int weatherID;
	public WeatherSync w;
	private bool lightOn = true;
	GameObject[] shadows;
	GameObject[] skylights;
	public GameObject snowProjector;
	public bool snowActive = false;

	//Skylight Colors
	public Transform lightProjector;
	private Material light;
	public Color day;
	public Color twilight;
	public Color night;
	public Color cloudyDay;

	//Weather Effects
	private int previousConditionID = -1000;
	private int currentConditionID = 0;
	public string conditionName = "clear";
	private ParticleSystem currentWeatherEffect;
	public ParticleSystem rain;
	//public ParticleSystem clouds;
	public ParticleSystem snow;
	public ParticleSystem hail;
	public ParticleSystem snowStorm;
	public ParticleSystem rainStorm;
	public ParticleSystem fog;

	//Weather Sounds
	public AudioClip drizzleLoop;
	public AudioClip rainLoop;
	public AudioClip windLoop;

	//Weather ID Ranges
	private int stormMin = 200;
	private int stormMax = 232;
	private int drizzleMin = 300;
	private int drizzleMax = 321;
	private int rainMin = 500;
	private int rainMax = 531;
	private int clearMin = 800;
	private int clearMax = 801;
	//private int cloudMin = 802;
	//private int cloudMax = 804;
	private int snowMin = 600;
	private int snowMax = 601;
	private int hailMin = 906;
	private int hailMax = 906;
	private int snowStormMin = 602;
	private int snowStormMax = 622;
	private int fogMin = 701;
	private int fogMax = 741;

	// Use this for initialization
	void Start() {
		w = GameObject.FindGameObjectWithTag ("Weather").GetComponent<WeatherSync>();
		light = lightProjector.GetComponent<Projector>().material;
		shadows = GameObject.FindGameObjectsWithTag("Shadow");
		skylights = GameObject.FindGameObjectsWithTag("Sunlight");
		currentConditionID = w.conditionID.GetValue();
		//Debug.Log (w.conditionID);

		updateSkylightColor();
		w.isIndoors = isIndoors;
		if(!isIndoors) {
			updateWeatherEffects();
			if(snowProjector != null) {
				bool snow = (currentConditionID >= snowMin && currentConditionID <= snowMax);
				bool snowStorm = currentConditionID >= snowStormMin && currentConditionID <= snowStormMax;
				if(snow||snowStorm) {
					snowProjector.SetActive(true);
					snowActive = true;
					w.inSnow = true;
				}
				else if(snowProjector != null) {
					snowProjector.SetActive(false);
					snowActive = false;
					w.inSnow = false;
				}

			}
			else {
				snowActive = false;
				w.inSnow = false;
			}
		}

	}

	void Update() {
		if(!isIndoors) {
			currentConditionID = w.conditionID.GetValue();
			if(previousConditionID != currentConditionID) {
				previousConditionID = currentConditionID;
				updateWeatherEffects();
			}
		}

		updateLightVisibility();
	}

	public void updateLightVisibility() {
		if(lightOn&&w.lightMax.GetValue() < 1) { 
			foreach(GameObject skylight in skylights) {
				if(skylight.name == "Skylight") skylight.SetActive(false);
			}
			foreach(GameObject shadow in shadows) {
				shadow.SetActive(false);
			}
			lightOn = false;
		}
		else if (!lightOn && w.lightMax.GetValue() > 0) {
			foreach(GameObject skylight in skylights) {
				if(skylight.name == "Skylight") skylight.SetActive(true);
			}
			foreach(GameObject shadow in shadows) {
				shadow.SetActive(true);
			}
			lightOn = true;
		}
	}
	
	public void updateWeatherEffects() {

		//Rain
		//Split it up here to treat rain and drizzle as the same thing
		bool isRaining = currentConditionID >= rainMin && currentConditionID <= rainMax;
		bool isDrizzling = currentConditionID >= drizzleMin && currentConditionID <= drizzleMax;
		if (isRaining||isDrizzling) {
			if(currentConditionID < 502) adjustEmission(50, rain);
			else adjustEmission(100, rain);
			swapWeather(rain, "Rain", drizzleLoop);
		}
		//heavy storms
		else if(currentConditionID >= stormMin && currentConditionID <= stormMax) {
			adjustEmission(250, rainStorm);
			swapWeather(rainStorm, "Storm", rainLoop);
		}
		//Ordinary, gentle, snowfall
		else if(currentConditionID >= snowMin && currentConditionID <= snowMax) {
			adjustEmission(30, snow);
			swapWeather(snow, "Snow");
		}
		//snowstorms
		else if(currentConditionID >= snowStormMin && currentConditionID <= snowStormMax) {
			adjustEmission(200, snowStorm);
			swapWeather(snowStorm, "SnowStorm", windLoop);
		}
		//fog
		else if(currentConditionID >= fogMin && currentConditionID <= fogMax) {
			adjustEmission(1, fog);
			swapWeather(fog, "Fog");
		}
		//hail
		else if(currentConditionID >= hailMin && currentConditionID <= hailMax) {
			adjustEmission(200, hail);
			swapWeather(hail, "Hail");
		}
		//Otherwise, assume it's clear outside
		else {
			if(currentWeatherEffect != null) {
				audio.clip = null;
				currentWeatherEffect.Stop();
				audio.Stop();
			}
			currentWeatherEffect = null;
			conditionName = "Clear";
		}
	}

	void adjustEmission(int mildE, ParticleSystem p) {
		float multiplier;
		if(currentConditionID%10 == 1 || currentConditionID%10 == 0) multiplier = mildE;
		else multiplier = mildE*10f;
		p.emissionRate = multiplier*transform.localScale.x;
	}

	void swapWeather(ParticleSystem weather, string name, AudioClip sound=null) {
		Debug.Log (name);
		if(currentWeatherEffect != null && currentWeatherEffect != weather) currentWeatherEffect.Stop();
		if(!rain.isPlaying)weather.Play();
		currentWeatherEffect = weather;
		conditionName = name;
		//swap out audio
		audio.Stop();
		audio.clip = sound;
		audio.Play();
	}

	public void updateSkylightColor() {
		if(w.isNightTime == false) {
			if(w.cloudinessPercentage.GetValue() >= 50) light.SetColor("_Color", cloudyDay);
			else if(w.lightMax.GetValue() >= 1 && w.lightMax.GetValue() < 4) light.SetColor("_Color", twilight);
			else if(w.lightMax.GetValue() >= 4) light.SetColor("_Color", day);
		}
		else light.SetColor("_Color", night);
	}
}
