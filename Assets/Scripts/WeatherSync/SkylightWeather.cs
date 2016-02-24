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
	public string conditionName = "clear";
	private ParticleSystem currentWeatherEffect;
	public ParticleSystem rain;
	public ParticleSystem clouds;
	public ParticleSystem snow;

	//Weather ID Ranges
	private int drizzleMin = 300;
	private int drizzleMax = 321;
	private int rainMin = 200;
	private int rainMax = 531;
	private int clearMin = 800;
	private int clearMax = 801;
	private int cloudMin = 802;
	private int cloudMax = 804;
	private int snowMin = 600;
	private int snowMax = 622;

	// Use this for initialization
	void Start() {
		w = GameObject.FindGameObjectWithTag ("Weather").GetComponent<WeatherSync>();
		light = lightProjector.GetComponent<Projector>().material;
		shadows = GameObject.FindGameObjectsWithTag("Shadow");
		skylights = GameObject.FindGameObjectsWithTag("Sunlight");

		if(!isIndoors) {
			updateWeatherEffects();
			if(snowProjector != null) {
				if(conditionName == "Snow") {
					snowProjector.SetActive(true);
					snowActive = true;
				}
				else if(snowProjector != null) {
					snowProjector.SetActive(false);
					snowActive = false;
				}

			}
			else snowActive = false;
		}

	}

	void Update() {
		if(!isIndoors) updateWeatherEffects();
		updateSkylightColor();
		updateLightVisibility();
	}

	public void updateLightVisibility() {
		if(lightOn&&w.lightMax < 1) { 
			foreach(GameObject skylight in skylights) {
				if(skylight.name == "Skylight") skylight.SetActive(false);
			}
			foreach(GameObject shadow in shadows) {
				shadow.SetActive(false);
			}
			lightOn = false;
		}
		else if (!lightOn && w.lightMax > 0) {
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
		bool isRaining = w.conditionID >= rainMin && w.conditionID <= rainMax;
		bool isDrizzling = w.conditionID >= drizzleMin && w.conditionID <= drizzleMax;
		if (isRaining||isDrizzling) {
			adjustEmission(50, rain);
			swapWeather(rain, "Rain");
		}
		//Clouds
//		else if(w.conditionID >= cloudMin && w.conditionID <= cloudMax) {
//			if(currentWeatherEffect != null && currentWeatherEffect != clouds) currentWeatherEffect.Stop();
//			if(!clouds.isPlaying)clouds.Play();
//			currentWeatherEffect = clouds;
//		}
		else if(w.conditionID >= snowMin && w.conditionID <= snowMax) {
			adjustEmission(200, snow);
			swapWeather(snow, "Snow");
		}
		else {
			if(currentWeatherEffect != null) currentWeatherEffect.Stop ();
			currentWeatherEffect = null;
			conditionName = "Clear";
		}
	}

	void adjustEmission(int mildE, ParticleSystem p) {
		float multiplier;
		if(w.conditionID%10 == 1 || w.conditionID%10 == 0) multiplier = mildE;
		else multiplier = mildE*10f;
		p.emissionRate = multiplier*transform.localScale.x;
	}

	void swapWeather(ParticleSystem weather, string name) {
		if(currentWeatherEffect != null && currentWeatherEffect != weather) currentWeatherEffect.Stop();
		if(!rain.isPlaying)weather.Play();
		currentWeatherEffect = weather;
		conditionName = name;
	}

	public void updateSkylightColor() {
		if(w.isNightTime == false) {
			if(w.cloudinessPercentage >= 50) light.SetColor("_Color", cloudyDay);
			else if(w.lightMax >= 1 && w.lightMax < 4) light.SetColor("_Color", twilight);
			else if(w.lightMax >= 4) light.SetColor("_Color", day);
		}
		else light.SetColor("_Color", night);
	}
}
