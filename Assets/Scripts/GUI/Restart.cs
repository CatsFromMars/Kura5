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
}
