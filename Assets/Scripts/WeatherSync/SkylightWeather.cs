using UnityEngine;
using System.Collections;

public class SkylightWeather : MonoBehaviour {

	private int weatherID;
	public WeatherSync w;
	//Wheather Effects
	private ParticleSystem currentWeatherEffect;
	public ParticleSystem rain;
	public ParticleSystem clouds;
	public ParticleSystem snow;

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
		updateWeatherEffects();
	}
	
	public void updateWeatherEffects() {

		//Rain
		if (w.conditionID >= rainMin && w.conditionID <= rainMax) {
			if(currentWeatherEffect != null) currentWeatherEffect.Stop();
			rain.Play();
			currentWeatherEffect = rain;
		}
		//Clouds
		else if(w.conditionID >= cloudMin && w.conditionID <= cloudMax) {
			if(currentWeatherEffect != null) currentWeatherEffect.Stop();
			clouds.Play();
			currentWeatherEffect = clouds;
		}
		else {
			if(currentWeatherEffect != null) currentWeatherEffect.Stop ();
			currentWeatherEffect = null;
		}
	}
}
