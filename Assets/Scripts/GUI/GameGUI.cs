using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour {
	GameObject globalData;
	GameData gameData;
	public Texture2D lifeTexture;
	public Material lifeMat;
	public Texture2D energyTexture;
	public Texture2D annieEnergyTexture;
	public Texture2D emilEnergyTexture;
	public Material energyMat;
	public Material emilEnergy;
	public Material annieEnergy;
	float Lx = 107f;
	float Ly = 28f;
	float Lw = Screen.width * 100f * 0.00228f;
	float Lh = Screen.height * 0.029f;
	float Ex = 107f;
	float Ey = 58f;
	float Ew = Screen.width * 100f * 0.00228f;
	float Eh = Screen.height * 0.029f;
	float lifeRatio;
	float energyRatio;
	float currentLife;
	float currentEnergy;
	float maxLife;
	float maxEnergy;
	
	void Awake() {
		globalData = GameObject.FindGameObjectWithTag("GameController");
		gameData = globalData.GetComponent<GameData>();
	}

	void Update() {
		if(gameData.currentPlayer == GameData.player.Annie) {
			currentLife = gameData.annieCurrentLife;
			currentEnergy = gameData.annieCurrentEnergy;
			maxLife = gameData.annieMaxLife;
			maxEnergy = gameData.annieMaxEnergy;
			energyTexture = annieEnergyTexture;
			energyMat = annieEnergy;
		}
		if(gameData.currentPlayer == GameData.player.Emil) {
			currentLife = gameData.emilCurrentLife;
			currentEnergy = gameData.emilCurrentEnergy;
			maxLife = gameData.emilMaxLife;
			maxEnergy = gameData.emilMaxEnergy;
			energyTexture = emilEnergyTexture;
			energyMat = emilEnergy;

		}

		lifeRatio = (currentLife/maxLife);

		energyRatio = (currentEnergy/maxEnergy);

	}

	void OnGUI () {

		if(Event.current.type.Equals (EventType.Repaint)) { //ONLY WHEN STUFF IS BEING DRAWN...
			Rect lifeBox = new Rect(Lx, Ly, Lw*lifeRatio, Lh);
			Graphics.DrawTexture (lifeBox, lifeTexture, lifeMat);

			Rect energyBox = new Rect(Ex, Ey, Ew*energyRatio, Eh);
			Graphics.DrawTexture (energyBox, energyTexture, energyMat);
		}
	}
	
}
