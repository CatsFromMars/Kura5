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
		AudioListener.volume = 0;
	}

	IEnumerator load() {
		Time.timeScale = 0;
		while(w.finalTemp.GetValue() != -1000) yield return null;
		yield return CoroutineUtil.WaitForRealSeconds (2);
		fader.gotoScene (initialLevel);
	}
}
