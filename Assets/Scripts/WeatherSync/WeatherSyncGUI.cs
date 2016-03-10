using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeatherSyncGUI : MonoBehaviour {
	//Data
	private LightLevels lightLevels;
	public WeatherSync w;
	Text text;
	public GameObject canUseOverlay;

	//colors
	public Color lunaColor;
	public Color solColor;
	public Color darkColor;

	//emblems
	public Sprite lunaEmblem;
	public Sprite darkEmblem;
	public Sprite solEmblem;

	//WeatherSync attributes
	public Text temp;
	public Text humidity;
	public Text cloud;
	public Text status;
	public Text clock;

	public SpriteRenderer emblem;
	public Slider slider;
	public Image fill;

	//Audio
	private AudioSource audio;
	public AudioClip light;
	public AudioClip dark;


	// Use this for initialization
	void Awake () {
		audio = GetComponent<AudioSource> ();
		text = GetComponent<Text>();
		lightLevels = GameObject.FindGameObjectWithTag("LightLevels").GetComponent<LightLevels>();
	}

	void Update() {
		updateUI ();
	}

	void updateUI() {
		//Update Values
		string c = (w.cloudinessPercentage).ToString();
		cloud.text = c+"%";
		temp.text = w.finalTemp + " C";
		string h = w.humidityPercentage.ToString ();
		humidity.text = h + "%";
		clock.text = System.DateTime.Now.ToString ("hh:mm tt");
		status.text = w.status;

		//Update Slider
		if(lightLevels.darkness > 0) {
			fill.color = darkColor;
			emblem.sprite = darkEmblem;
			slider.value = lightLevels.darkness;
			if(canUseOverlay.gameObject.activeSelf) makeSound(dark);
			canUseOverlay.SetActive(false);
		}
		else if (w.isNightTime) {
			fill.color = lunaColor;
			emblem.sprite = lunaEmblem;
			slider.value = w.lightMax;
			if(lightLevels.sunlight > 0) {
				if(canUseOverlay.gameObject.activeSelf) makeSound(light);
				canUseOverlay.SetActive(false);
			}
			else canUseOverlay.SetActive(true);
		}
		else if (w.isNightTime == false) { //Daytime
			fill.color = solColor;
			emblem.sprite = solEmblem;
			slider.value = w.lightMax;
			if(lightLevels.sunlight > 0) {
				if(canUseOverlay.gameObject.activeSelf) makeSound(light);
				canUseOverlay.SetActive(false);
			}
			else canUseOverlay.SetActive(true);
		}
		else {
			canUseOverlay.SetActive(true);
		}

	}

	public void makeSound(AudioClip clip) {
		//ANIMATION EVENTS FOR ALL THINGS THAT NEED SOUND
		audio.clip = clip;
		audio.Play();
	}
}
