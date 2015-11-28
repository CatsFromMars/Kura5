using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {
	public string initialLevel = "";
	// Use this for initialization
	void Awake () {
		SceneTransition fader = GameObject.FindGameObjectWithTag("Fader").GetComponent<SceneTransition>();
		fader.gotoScene (initialLevel);
	}
}
