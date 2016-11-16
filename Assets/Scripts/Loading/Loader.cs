using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {
	public string initialLevel = "";
	public WeatherSync w;
	SceneTransition fader;
	public Transform playerContainer;
	// Use this for initialization
	void Start() {
		fader = GameObject.FindGameObjectWithTag("Fader").GetComponent<SceneTransition>();
		StartCoroutine(load());
		AudioListener.volume = 0;
	}

	IEnumerator load() {
		if(SaveLoad.Load()) {
			yield return CoroutineUtil.WaitForRealSeconds(0.01f);
			Time.timeScale = 0;
			while(w.finalTemp.GetValue() == -1000) yield return null;
			yield return CoroutineUtil.WaitForRealSeconds(2);
			//Place character in position
			Vector3 pos = new Vector3 (SaveLoad.state.playerLocationX,SaveLoad.state.playerLocationY,SaveLoad.state.playerLocationZ);
			playerContainer.transform.position = pos;
			if(SaveLoad.state.currentScene!="") fader.gotoScene (SaveLoad.state.currentScene);
			else fader.gotoScene (initialLevel);
		}
		else StartCoroutine(newGame());
	}


	IEnumerator newGame() {
		Time.timeScale = 0;
		while(w.finalTemp.GetValue() == -1000) yield return null;
		yield return CoroutineUtil.WaitForRealSeconds (2);
		fader.gotoScene (initialLevel);
	}
}
