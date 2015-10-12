using UnityEngine;
using System.Collections;

public class Restart : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) resetScene();
						
	}

	void resetScene() {
		Application.LoadLevel (Application.loadedLevel);
	}

	void resetGame() {
		GameObject global = GameObject.FindGameObjectWithTag ("Global");
		Destroy (global.gameObject);
		Application.LoadLevel ("MenuScene");
	}
}
