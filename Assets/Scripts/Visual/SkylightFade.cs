using UnityEngine;
using System.Collections;

public class SkylightFade : MonoBehaviour {
	public Color day;
	public Color twilight;
	public Color night;
	private Color currentColor;
	private Material light;
	public WeatherSync w;
	public LightLevels lightLevels;
	private float currentAlpha = 0;

	// Update is called once per frame
	void Awake() {
		light = GetComponent<SpriteRenderer>().material;
		lightLevels = GameObject.FindGameObjectWithTag ("LightLevels").GetComponent<LightLevels>();
		w = GameObject.FindGameObjectWithTag ("Weather").GetComponent<WeatherSync>();
	}

	void Update() {
		if(lightLevels.distanceSun < lightLevels.radius*2) fadeIn();
		else fadeOut();
		updateSkylightColor();
	}

	void fadeIn() {
		currentAlpha += Time.deltaTime;
	}

	void fadeOut() {
		currentAlpha = Mathf.Lerp(currentAlpha, 0, 5*Time.deltaTime);
	}

	public void updateSkylightColor() {
		if(w.isNightTime == false) {
			if(w.lightMax >= 1 && w.lightMax < 4) currentColor = twilight;
			else if(w.lightMax >= 4) currentColor = day;
		}
		else currentColor = night;
		currentColor.a = currentAlpha;
		light.SetColor ("_TintColor", currentColor);
	}
}
