using UnityEngine;
using System.Collections;

public class GetActivePlayer : MonoBehaviour {

	public static GameObject getActivePlayer() {
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject p in players) {
			if(p.activeSelf) return p;
		}
		return null;
	}
}
