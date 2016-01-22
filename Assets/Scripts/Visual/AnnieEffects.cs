using UnityEngine;
using System.Collections;

public class AnnieEffects : MonoBehaviour {
	//Weather related visual effects to be used on Annie or any other humans
	private WeatherSync w;
	public GameObject breath;

	// Use this for initialization
	void Start () {
		w = GameObject.FindGameObjectWithTag("Weather").GetComponent<WeatherSync>();
	}
	
	// Update is called once per frame
	void Update () {
		if(w.finalTemp <= 0) breath.SetActive(true);
		else breath.SetActive(false);
	}
}
