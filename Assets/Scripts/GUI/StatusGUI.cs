using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusGUI : MonoBehaviour {
	public Color solEnergy;
	public Color darkEnergy;
	public Slider lifeSlider;
	public Slider energySlider;
	public Image energyColor; 
	GameObject globalData;
	GameData gameData;

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
	}
}
