using UnityEngine;
using System.Collections;

public class Settings : MenuClass {

	public Color confirmColor;
	public Color origColor;
	public TextMesh weatherOn;
	public TextMesh weatherOff;
	public TextMesh speedNormal;
	public TextMesh speedFast;

	void Start() {
		setWeatherColor();
	}

	public override void ChooseOption() {
		if(!exiting) {
			if (index == 0) {
				//WeatherSync
				toggleWeather();
				return;
			}
			else if(index == 1) {
				//Quit
				ExitMenu();
			}
		}
	}

	void setWeatherColor() {
		if(PlayerPrefs.GetInt("WeatherSync") == 1) {
			weatherOn.color = confirmColor;
			weatherOff.color = origColor;
		}
		else {
			weatherOn.color = origColor;
			weatherOff.color = confirmColor;
		}
	}

	void toggleWeather() {
		if(PlayerPrefs.GetInt("WeatherSync") != 1) {
			PlayerPrefs.SetInt("WeatherSync", 1);
			setWeatherColor();
		}
		else {
			PlayerPrefs.SetInt("WeatherSync", 0);
			setWeatherColor();
		}
	}

	public override void ExitMenu() {
		makeSound(deny);
		Application.LoadLevel("MenuScene");
	}
}
