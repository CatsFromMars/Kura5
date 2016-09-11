using UnityEngine;
using System.Collections;

public class GetUtil : MonoBehaviour {

	public static GameData getData() {
		return GameObject.FindGameObjectWithTag("GameController").GetComponent<GameData>();
	}

	public static GameObject getPlayer() {
		return GameObject.FindGameObjectWithTag("PlayerSwapper");
	}
}
