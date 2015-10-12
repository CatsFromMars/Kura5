using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public GameObject start;
	public GameObject copyrightText;

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.Return)) {
			Application.LoadLevel ("LoadingScene"); 

		}
	
	}

	void enableText() {
		start.active = true;
		copyrightText.active = true;
	}
}
