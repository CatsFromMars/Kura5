using UnityEngine;
using System.Collections;

public class GetUtil : MonoBehaviour {

	public static GameData getData() {
		return GameObject.FindGameObjectWithTag("GameController").GetComponent<GameData>();
	}

	public static GameObject getPlayer() {
		return GameObject.FindGameObjectWithTag("PlayerSwapper");
	}

	public static PlayerContainer getPlayerContainer() {
		return GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContainer>();
	}

	public static Flags getFlags() {
		GameData data = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameData>();
		return data.gameObject.GetComponent<Flags>();
	}

	public static Inventory getInventory() {
		GameData data = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameData>();
		return data.gameObject.GetComponent<Inventory>();
	}

	public static WeatherSync getWeather() {
		return GameObject.FindGameObjectWithTag ("Weather").GetComponent<WeatherSync>();
	}
}
