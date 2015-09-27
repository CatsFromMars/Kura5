using UnityEngine;
using System.Collections;

public class SkylightWeather : MonoBehaviour {

	private int weatherID;
	public WeatherSync w;
	//Wheather Effects
	private ParticleSystem currentWeatherEffect;
	public ParticleSystem rain;

	private int rainMin = 200;
	private int rainMax = 531;
	private int clearMin = 800;
	private int clearMax = 802;
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
			//if(currentWeatherEffect != null) currentWeatherEffect.Stop();
			rain.Play();
			currentWeatherEffect = rain;
		}
		else {
			if(currentWeatherEffect != null) currentWeatherEffect.Stop ();
		}
	}
}
