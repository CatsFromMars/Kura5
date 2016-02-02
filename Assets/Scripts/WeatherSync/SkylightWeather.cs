using UnityEngine;
using System.Collections;

public class SkylightWeather : MonoBehaviour {

	private int weatherID;
	public WeatherSync w;
	private bool lightOn = true;
	GameObject[] shadows;
	GameObject[] skylights;

	//Skylight Colors
	public Transform lightProjector;
	private Material light;
	public Color day;
	public Color twilight;
	public Color night;

	//Weather Effects
	private ParticleSystem currentWeatherEffect;
	public ParticleSystem rain;
	public ParticleSystem clouds;
	public ParticleSystem snow;

	//Weather ID Ranges
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
	}

	void Update() {
		updateWeatherEffects();
		updateSkylightColor();
	}

	public void updateLightVisibility() {
		if(lightOn&&w.lightMax < 1) { 
			foreach(GameObject skylight in skylights) {
				if(skylight.name == "Skylight") skylight.SetActive(false);
			}
			foreach(GameObject shadow in shadows) {
				shadow.SetActive(false);
			}
		}
		else if (!lightOn && w.lightMax > 0) {
			foreach(GameObject skylight in skylights) {
				if(skylight.name == "Skylight") skylight.SetActive(true);
			}
			foreach(GameObject shadow in shadows) {
				shadow.SetActive(true);
			}
		}
	}
	
	public void updateWeatherEffects() {

		//Rain
		if (w.conditionID >= rainMin && w.conditionID <= rainMax) {
			if(currentWeatherEffect != null && currentWeatherEffect != rain) currentWeatherEffect.Stop();
			if(!rain.isPlaying)rain.Play();
			currentWeatherEffect = rain;
		}
		//Clouds
		else if(w.conditionID >= cloudMin && w.conditionID <= cloudMax) {
			if(currentWeatherEffect != null && currentWeatherEffect != clouds) currentWeatherEffect.Stop();
			if(!clouds.isPlaying)clouds.Play();
			currentWeatherEffect = clouds;
		}
		else {
			if(currentWeatherEffect != null) currentWeatherEffect.Stop ();
			currentWeatherEffect = null;
		}
	}

	public void updateSkylightColor() {
		if(w.isNightTime == false) {
			if(w.lightMax >= 1 && w.lightMax < 4) light.SetColor("_Color", twilight);
			else if(w.lightMax >= 4) light.SetColor("_Color", day);
		}
		else light.SetColor("_Color", night);
	}
}
