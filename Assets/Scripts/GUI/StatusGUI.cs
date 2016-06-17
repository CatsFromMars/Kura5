using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusGUI : MonoBehaviour {
	public Color fullColor;
	public Color halfColor;
	public Color lowColor1;
	public Color lowColor2;
	public Color solEnergy;
	public Color darkEnergy;
	public Slider lifeSlider;
	public Slider energySlider;
	public Image lifeColor;
	public Image energyColor; 
	GameObject globalData;
	GameData gameData;
	int currentLife = 0;
	int currentEnergy = 0;
	private bool lowHP = false;

	void Awake() {
		globalData = GameObject.FindGameObjectWithTag("GameController");
		gameData = globalData.GetComponent<GameData>();
	}
	
	// Update is called once per frame
	void Update () {
		if (gameData.currentPlayer == GameData.player.Annie) {
			lifeSlider.value = gameData.annieCurrentLife;
			energySlider.value = gameData.annieCurrentEnergy;
			energyColor.color = solEnergy;
		}
		else if (gameData.currentPlayer == GameData.player.Emil) {
			lifeSlider.value = gameData.emilCurrentLife;
			energySlider.value = gameData.emilCurrentEnergy;
			energyColor.color = darkEnergy;
		}

		updateLifeBarColor ();
	}

	void updateLifeBarColor() {
		if((lifeSlider.value <= (lifeSlider.maxValue/4f)) && !lowHP) {
			lowHP = true;
			StartCoroutine(redFlash());
		}
		else if (lifeSlider.value <= (lifeSlider.maxValue/2f) && (lifeSlider.value >= (lifeSlider.maxValue/4f))) {
			lifeColor.color = halfColor;
			lowHP = false;
		}
		else if (lifeSlider.value > (lifeSlider.maxValue/2f)) {
			lifeColor.color = fullColor;
			lowHP = false;
		}
	}

	IEnumerator redFlash() {
		while(lowHP) {
			lifeColor.color = lowColor1;
			yield return new WaitForSeconds (0.08f);
			lifeColor.color = lowColor2;
			yield return new WaitForSeconds (0.08f);
		}
	}
}
