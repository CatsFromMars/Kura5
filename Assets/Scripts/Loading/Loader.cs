using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {
	public string initialLevel = "";
	public WeatherSync w;
	SceneTransition fader;
	// Use this for initialization
	void Awake () {
		fader = GameObject.FindGameObjectWithTag("Fader").GetComponent<SceneTransition>();
		StartCoroutine(load());
	}

	IEnumerator load() {
		Time.timeScale = 0;
		yield return w.conditionName != "" && w.status != "";
		yield return CoroutineUtil.WaitForRealSeconds (2);
		fader.gotoScene (initialLevel);
	}
}
